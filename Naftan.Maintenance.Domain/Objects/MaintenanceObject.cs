using System;
using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Usage;
using Naftan.Common.Domain.EntityComponents;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Объект обслуживания
    /// </summary>
    public class MaintenanceObject : TreeNode<MaintenanceObject>, IEntity
    {
        protected MaintenanceObject() { }

        private readonly ICollection<ObjectSpecification> specifications = new HashSet<ObjectSpecification>();
        private readonly ICollection<ObjectOperatingState> operatingStates = new HashSet<ObjectOperatingState>();
        private readonly ICollection<LastMaintenance> lastMaintenance = new HashSet<LastMaintenance>();
        private readonly ICollection<MaintenanceActual> maintenance = new HashSet<MaintenanceActual>();
        private readonly ICollection<UsageActual> usage = new HashSet<UsageActual>();
        private readonly ICollection<MaintenancePlan> plans = new HashSet<MaintenancePlan>();

        public MaintenanceObject(ObjectGroup group, string techIndex, DateTime? startOperating, Period period = null, IEnumerable<LastMaintenance> last = null)
        {
            Group = group;
            TechIndex = techIndex;
            StartOperating = startOperating;
            ChangeOperatingState(OperatingState.Operating, startOperating??DateTime.Now);
            Report = new OperationalReport
            {
                MaintenanceObject = this,
                Period = period ?? Period.Now(),
                UsageBeforeMaintenance = period?.Hours() ?? Period.Now().Hours(),
                State = CurrentOperatingState.Value
            };

            if (last != null && last.Any())
            {
                last.ToList().ForEach(x =>
                {
                    x.Object = this;
                    lastMaintenance.Add(x);
                });
            }
            else{

                Intervals.ToList().ForEach(x =>
                {
                    lastMaintenance.Add(new LastMaintenance(
                        x.MaintenanceType,
                        null,
                        x.MinUsage != null ? (int?)0 : null
                        )
                    {
                        Object = this
                    });
                });
            }

            SetNextMaintenance();

        }

        #region Поля используемые для интерации данных из dbf
        public int? ReplicationKvo { get; private set; }
        public int? ReplicationKg { get; private set; }
        public int? ReplicationKu { get; private set; }
        public int? ReplicationKc { get; private set; }
        public int? ReplicationKmrk { get; private set; }

        /// <summary>
        /// Эксплуатируемость
        /// 0-"Эксплуатируется"
        /// 1-"Не эксплуатируется"
        /// </summary>
        public int? ReplicationFe { get; private set; } 
        #endregion

        public int Id { get; set; }
        /// <summary>
        /// Группа объекта ремонта
        /// </summary>
        public ObjectGroup Group { get; private set; }
        /// <summary>
        /// Установка
        /// </summary>
        public Plant Plant { get; set; }
        /// <summary>
        /// Рабочая среда
        /// </summary>
        [Obsolete]
        public int? EnvironmentId { get; private set; }
        /// <summary>
        /// Завод производитель
        /// </summary>
        [Obsolete]
        public int? ManufacturerId { get; private set; }
        /// <summary>
        /// Заводской номер
        /// </summary>
        [Obsolete]
        public string FactoryNumber { get; private set; }
        /// <summary>
        /// Инвентарный номер
        /// </summary>
        public string InventoryNumber { get; set; }
        /// <summary>
        /// Технологический индекс
        /// </summary>
        public string TechIndex { get; set; }

        /// <summary>
        /// Дата ввода в эксплуатацию
        /// </summary>
        public DateTime? StartOperating { get; private set; }

        #region Оперативный отчёт
        /// <summary>
        /// Оперативный отчёт о работе за месяц
        /// </summary>
        public OperationalReport Report { get; set; }

        /// <summary>
        /// Применить отчёт (добавить информацию о ремонтах и наработке в соответствующие журналы) и сформировать план на след. период
        /// </summary>
        public void ApplyReport()
        {

            if (CurrentOperatingState == null)
            {
                ChangeOperatingState(OperatingState.Operating, Report.Period.Start());
            }

            if (!LastMaintenance.Any())
            {
                Intervals.ToList().ForEach(x =>
                {
                    lastMaintenance.Add(new LastMaintenance(
                        x.MaintenanceType,
                        null,
                        x.MinUsage != null ? (int?)0 : null
                        )
                    {
                        Object = this
                    });
                });
            }

            var CurrentPeriod = Report.Period;
            var NextPeriod = CurrentPeriod.Next();
            
            //Если оборудование не эксплуатируется то в расчёт больше не берётся
            if (CurrentOperatingState == OperatingState.WriteOff) return;

            //Если оборудование установлено как не эксплуатируется, то меняем состояние и больше не берём в расчёт
            if (Report.State == OperatingState.WriteOff)
            {
                ChangeOperatingState(OperatingState.WriteOff, CurrentPeriod.End());
                return;
            }


            // 1 Добавить наработку до ремонта, а если его не было, то за весь период
            if (Report.UsageBeforeMaintenance != 0)
            {
                AddUsage(CurrentPeriod.Start(), Report.StartMaintenance ??CurrentPeriod.End(), Report.UsageBeforeMaintenance);
                Report.UsageBeforeMaintenance = 0;
            }

            //2 Добавить информацию о ремонте, если он был.
            
            // Если был неоконченный ремонт, то завершить его
            if (CurrentMaintenance != null)
            {
                FinalizeMaintenance(Report.EndMaintenance.Value);
            }
            // Если новый ремонт, то добавляем его в журнал ремонтов
            else if (Report.ActualMaintenanceType!=null && Report.StartMaintenance != null)
            {
                AddMaintenance(Report.ActualMaintenanceType, Report.StartMaintenance.Value, Report.EndMaintenance, Report.UnplannedReason);
            }

            //3 если ремонт окончен и наработка после ремонта есть то добавляем её в журнал наработки
            if (Report.EndMaintenance != null && Report.UsageAfterMaintenance != 0)
            {
                AddUsage(Report.EndMaintenance.Value, CurrentPeriod.End(), Report.UsageAfterMaintenance);
            }

            //Планируем следующий период
            PlanningMaintenance(NextPeriod, Report.OfferForPlan, Report.ReasonForOffer);
            //Рассчитываем следующее обслуживание
            SetNextMaintenance();


            /* Очищение информации в отчёте для следующего месяца */

            // Если ремонт окончен, то обнуляем информацию по фактическому ремонту
            if (Report.EndMaintenance != null)
            {
                Report.ActualMaintenanceType = null;
                Report.UnplannedReason = null;
                Report.StartMaintenance = null;
                Report.EndMaintenance = null;
            }

            Report.OfferForPlan = null;
            Report.ReasonForOffer = null;

            if (CurrentOperatingState == OperatingState.Operating)
            {
                Report.UsageBeforeMaintenance = NextPeriod.Hours();
            }
            else
            {
                Report.UsageBeforeMaintenance = 0;
            }
            
            Report.UsageAfterMaintenance = 0;
            Report.State = CurrentOperatingState.Value;

            //Переходим на следующий период
            Report.Period = NextPeriod;

                        
        }

        public void DiscardReport()
        {

        }


        #endregion


        #region Рабочее состояние

        /// <summary>
        /// Текущее рабочее состояние
        /// </summary>
        public OperatingState? CurrentOperatingState { get; private set; }

        /// <summary>
        /// История изменения состояния эксплуатации
        /// </summary>
        public IEnumerable<ObjectOperatingState> OperatingStates => operatingStates;

        /// <summary>
        /// Изменить рабочее состояние объекта ремонта
        /// </summary>
        /// <param name="state">Рабочее состояние</param>
        /// <param name="startDate">Дата изменения состояния</param>
        private void ChangeOperatingState(OperatingState state, DateTime startDate)
        {
            if (CurrentOperatingState != state)
            {
                CurrentOperatingState = state;
                operatingStates.Add(new ObjectOperatingState
                {
                    State = state,
                    Object = this,
                    StartDate = startDate
                });
            }
        }

        #endregion

        #region Технические характеристики

        /// <summary>
        /// Технические характеристики
        /// </summary>
        public IEnumerable<ObjectSpecification> Specifications => specifications;



        /// <summary>
        /// Добавить техническую характеристику
        /// </summary>
        /// <param name="specification"></param>
        public void AddSpecification(ObjectSpecification specification)
        {
            if (specifications.All(f => f.Specification.Id != specification.Specification.Id))
            {
                specification.Object = this;
                specifications.Add(specification);
            }
        }

        /// <summary>
        /// Удалить техническую характеристику
        /// </summary>
        /// <param name="specification"></param>
        public void RemoveSpecification(ObjectSpecification specification)
        {
            if (specifications.Any(f => f.Specification.Id == specification.Specification.Id))
            {
                specifications.Remove(specification);
            }
        }


                /// <summary>
        /// Добавить технические характеристики из группы (цепочки родителей)
        /// </summary>
        /// <param name="group"></param>
        public void AddSpecificationsFromGroup(ObjectGroup group = null)
        {

            var _group = (group ?? Group);

            _group.Specifications.ToList().ForEach(f => AddSpecification(new ObjectSpecification(f.Specification, f.DefaultValue)));
            if (_group.Parent != null)
                AddSpecificationsFromGroup(_group.Parent);
        }


        #endregion

        #region Наработка

        public IEnumerable<UsageActual> Usage => usage;

        public void AddUsage(DateTime start, DateTime end, int usage)
        {
            //todo вводимая наработка должная быть в пределах одного периода( потому что отчётный период ил период планирования 1 мес. (уточнить!))
            //todo проверить не вводилась ли наработка за этот период ранее

            //Добавить запись в журнал наработки
            var newUsage = new UsageActual
            {
                Object = this,
                StartUsage = start,
                EndUsage = end,
                Usage = usage
            };

            this.usage.Add(newUsage);

            //Добавить наработку в последние ремонты 
            lastMaintenance.ToList().ForEach(last =>
            {
                last.AddUsage(usage);
            });

            //Добавить наработку с начала эксплуатации
            UsageFromStartup  = (UsageFromStartup??0) + usage;
        }

        /// <summary>
        /// Наработка с начала экслуатации
        /// </summary>
        public int? UsageFromStartup { get; private set; }


        #endregion

        #region Обслуживание

        /// <summary>
        /// Последнее обслуживание
        /// </summary>
        public IEnumerable<LastMaintenance> LastMaintenance => lastMaintenance;

        /// <summary>
        /// Журнал Обслуживания
        /// </summary>
        public IEnumerable<MaintenanceActual> Maintenance => maintenance;

        /// <summary>
        /// Текущее обслуживание
        /// </summary>
        public MaintenanceActual CurrentMaintenance { get; private set; }

        /// <summary>
        /// Межремонтные интервалы беруться через группу
        /// </summary>
        public IEnumerable<MaintenanceInterval> Intervals
        {
            get
            {
                var group = Group;
                while (!group.Intervals.Any())
                {
                    group = group.Parent;
                    if (group == null)
                        //Если интервалы не найдены возвращаем пустую коллекцию
                        return new List<MaintenanceInterval>();
                }
                return group.Intervals;
            }
        }

        public void AddMaintenance(
           MaintenanceType maintenanceType,
           DateTime start,
           DateTime? end = null,
           MaintenanceReason unplannedReason = null
           )
        {
            var targetInterval = Intervals.SingleOrDefault(x => x.MaintenanceType == maintenanceType);

            if (targetInterval == null)
            {
                throw new Exception("Не найден интервал для данного вида обслуживания");
            }

            //Добавить запись в журнал обслуживания
            var newMaintenance = new MaintenanceActual
            {
                Object = this,
                MaintenanceType = maintenanceType,
                StartMaintenance = start,
                UnplannedReason = unplannedReason
            };

            maintenance.Add(newMaintenance);

            //Изменить состояние объекта ремонта 
            ChangeOperatingState(OperatingState.Maintenance, start);
            CurrentMaintenance = newMaintenance;

            //Если обслуживание окончено (известна дата окончания) то производим процедуру завершения обслуживания
            if (end != null)
            {
                FinalizeMaintenance(end.Value);
            }

        }

        public void FinalizeMaintenance(DateTime end)
        {
            if (CurrentOperatingState == OperatingState.Maintenance)
            {
                //Сохраняем дату завершения обслуживания
                CurrentMaintenance.EndMaintenance = end;

                //Взять интересующий интервал по типу обслуживания
                var targetInterval = Intervals.SingleOrDefault(x => x.MaintenanceType == CurrentMaintenance.MaintenanceType);


                /* Выбрать интервалы для сброса. Сюда попадает интересующий нас интервал и более мелкие.
                 * Т.е. Если проводится Средний ремонт, то сбрасываются Средний, Текущий и Обслуживание.
                */

                //todo В данном случае выбираетя более мелкий интервал по количеству в ремонтном цикле,а если количества нет?
                var intervalsForReset = Intervals.Where(
                    x => x.QuantityInCycle >= targetInterval.QuantityInCycle)
                    .ToDictionary(i => i.MaintenanceType);

                //Если последнего обслуживания нет, то добавляем его
                if (lastMaintenance.All(x => x.MaintenanceType != CurrentMaintenance.MaintenanceType))
                {
                    var newLastMaintenance = new LastMaintenance(
                            CurrentMaintenance.MaintenanceType,
                            end,
                            UsageFromStartup
                        );
                    newLastMaintenance.Object = this;

                    lastMaintenance.Add(newLastMaintenance);
                }

                //Сбрасываем данные по последнему обслуживанию
                lastMaintenance.ToList().ForEach(last =>
                {
                    if (intervalsForReset.ContainsKey(last.MaintenanceType))
                    {
                        last.Reset(end);
                    }
                });

                ChangeOperatingState(OperatingState.Operating, end);
                CurrentMaintenance = null;
            }
        }

        #endregion

        #region Запчасти

        //todo запчасти

        #endregion

        #region Планирование

        /// <summary>
        /// Предложения к плану
        /// </summary>
        public IEnumerable<MaintenancePlan> Plans => plans;

        /// <summary>
        /// Вид обслуживания, который будет проведён следующим
        /// </summary>
        public MaintenanceType NextMaintenance { get; private set; }
        /// <summary>
        /// Наработка для следующего обслуживания (норма)
        /// </summary>
        public int? NextUsageNorm { get; private set; }
        /// <summary>
        /// Наработка для следующего обслуживания (факт)
        /// </summary>
        public int? NextUsageFact { get; private set; } 

        /// <summary>
        /// Установка следующего обслуживания(прогноз)
        /// </summary>
        private void SetNextMaintenance()
        {
            //чтобы рассчитать следующее обслуживание нужно сравнить по всем обслуживаниям разницу наработки норма-факт и взять меньшую разницу
            var intervals = Intervals.ToList();
            intervals.Sort();
            
            var intervalsMap = intervals.ToDictionary(x => x.MaintenanceType.Id);
            var lastUsageMap = LastMaintenance.ToDictionary(x => x.MaintenanceType.Id, x => x.UsageFromLastMaintenance);

            MaintenanceType next = null;
            int? fact=0;
            int? norm=0;

            intervals.ForEach(interval =>
            {
                var usage = lastUsageMap[interval.MaintenanceType.Id];
                if (
                    next == null ||
                    //сравниваем значения как по величине так и по модулю, на случай если ремонт пропущен и разница минусовая
                    (interval.MinUsage - usage < norm - fact &&
                        Math.Abs(interval.MinUsage.Value - usage.Value) < Math.Abs(norm.Value - fact.Value))
                )
                {
                    next = interval.MaintenanceType;
                    norm = interval.MinUsage;
                    fact = usage;
                }
            });

            NextMaintenance = next;
            NextUsageFact = fact;
            NextUsageNorm = norm;
        }


        /// <summary>
        /// Спланировать работы по обслуживанию
        /// </summary>
        /// <param name="period">Период планирования</param>
        /// <param name="offer">Предложение обслуживания</param>
        /// <param name="offerReason">Причина предложения</param>
        public void PlanningMaintenance(Period period, MaintenanceType offer, MaintenanceReason offerReason)
        {
            ///сортируем интервалы по величине ремонта (самый крупный будет первым)
            var intervals = Intervals.ToList();
            intervals.Sort();

            var intervalsMap = intervals.ToDictionary(x => x.MaintenanceType.Id);
            var lastDateMap = LastMaintenance.ToDictionary(x => x.MaintenanceType.Id, x => x.LastMaintenanceDate);
            var lastUsageMap = LastMaintenance.ToDictionary(x => x.MaintenanceType.Id, x=>x.UsageFromLastMaintenance);

            var hours = period.Hours();
            
            intervals.Any(interval =>
            {
                var type = interval.MaintenanceType.Id;

                if (!lastUsageMap.ContainsKey(type))
                {
                    var newLast = new LastMaintenance(interval.MaintenanceType, null, null)
                    {
                        Object = this
                    };

                    lastMaintenance.Add(newLast);
                    lastUsageMap.Add(type, 0);
                    lastDateMap.Add(type, null);
                }

                //проверка по наработке
                if (interval.MinUsage != null && (lastUsageMap[type].Value + hours) >= interval.MinUsage.Value)
                {
                    //дата проведения обслуживания
                    var date = period.Start().AddDays((interval.MinUsage.Value - lastUsageMap[type].Value) / 24);

                    //если дата ремонта выходит за пределы периода, то устонавливаем её в пределы периода
                    if (date > period.End()) date = period.End();
                    if (date < period.Start()) date = period.Start();

                    plans.Add(new MaintenancePlan
                    {
                        MaintenanceDate = date,
                        MaintenanceType = interval.MaintenanceType,
                        Object = this
                    });

                    return true;

                }

                //проверка по времени
                else if (interval.PeriodQuantity != null)
                {
                    //todo сделать проверку интервалов по времени
                }
                //проверка по предложению
                else if (offer != null && offer.Id==type)
                {

                    plans.Add(new MaintenancePlan
                    {
                        MaintenanceDate = period.Start(),
                        MaintenanceType = interval.MaintenanceType,
                        Object = this,
                        OfferReason = offerReason
                    });
                    return true;
                }

                return false;

            });
           
        }
        
        #endregion

    }
}