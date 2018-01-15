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
        private readonly ICollection<MaintenanceOffer> offers = new HashSet<MaintenanceOffer>();

        public MaintenanceObject(ObjectGroup group, string techIndex, DateTime startOperating, IEnumerable<LastMaintenance> last = null)
        {
            Group = group;
            TechIndex = techIndex;
            StartOperating = startOperating;
            ChangeOperatingState(OperatingState.Operating, startOperating);
            Report = new OperationalReport
            {
                MaintenanceObject = this,
                Period = Period.Now(),
                UsageBeforeMaintenance = Period.Now().Hours(),
                State = CurrentOperatingState.Value
            };

            if (last != null)
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
        /// Применить отчёт (добавить информацию о ремонтах и наработке в соответствующие журналы)
        /// </summary>
        public void ApplyReport()
        {

            if (CurrentOperatingState == null)
            {
                ChangeOperatingState(OperatingState.Operating, Report.Period.Start());
            }

            var CurrentPeriod = Report.Period;
            var NextPeriod = CurrentPeriod.Next();
            
            //Если оборудование списано то в расчёт больше не берётся
            if (CurrentOperatingState == OperatingState.WriteOff) return;

            //Если оборудование было списано, то меняем состояние и больше не берём в расчёт
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

            //Добавляем предложения для плана следующего периода
            if (Report.OfferForPlan != null)
            {
                offers.Add(new MaintenanceOffer
                {
                    Object = this,
                    MaintenanceType = Report.OfferForPlan,
                    Period = NextPeriod,
                    Reason = Report.ReasonForOffer
                });
            }


            /* Очищение информации в отчёте для следующего месяца */
                        
            // Если ремонт окончен, то обнуляем информацию по фактическому ремонту
            if (Report.EndMaintenance != null)
            {
                Report.ActualMaintenanceType = null;
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

            //Если у объекта есть действующий план, то проверяем остаётся ли он актуален
            if (CurrentPlan != null)
            {
                //Если срок плана истёк, то убираем его из объекта
                if (NextPeriod.Start() > CurrentPlan.EndDate)
                {
                    CurrentPlan = null;
                }
                //иначе добавляем планируемые работы в отчёт
                else
                {
                    /*todo 
                        здесь предполагается что планируется один вид работ на период, и более мелкие виды работ входят в состав более крупного ремонта, 
                        но возможно в будущем появится оборудование, у которого будет несколько видов работ за период, тогда нужно пересматривать вообще всю работу по оперативному отчёту
                     */
                    var plan = CurrentPlan.Details.SingleOrDefault(x => x.Object == this && x.MaintenanceDate >= NextPeriod.Start() && x.MaintenanceDate <= NextPeriod.End());
                    if (plan != null)
                    {
                        Report.PlannedMaintenanceType = plan.MaintenanceType;
                    }
                }
            }
            
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
                        throw new Exception("Не найдены межремонтные интервалы");
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
        public IEnumerable<MaintenanceOffer> Offers => offers;

        /// <summary>
        /// Действующий план
        /// </summary>
        public MaintenancePlan CurrentPlan { get; set; }


        /// <summary>
        /// Спланировать работы по обслуживанию
        /// </summary>
        /// <param name="plan">План</param>
        public void PlanningMaintenance(MaintenancePlan plan)
        {
            var intervals = Intervals.ToList();
            intervals.Sort();
            var intervalsMap = intervals.ToDictionary(x => x.MaintenanceType.Id);
            var lastDateMap = LastMaintenance.ToDictionary(x => x.MaintenanceType.Id, x => x.LastMaintenanceDate);
            var lastUsageMap = LastMaintenance.ToDictionary(x => x.MaintenanceType.Id, x=>x.UsageFromLastMaintenance);

            //Проверить можно ли планировать, существует ли текущий план, можно ли планировать на указанный период?

            //Удаляем из плана работы, запланированные ранее (в случае пересчёта плана)
            plan.Details.ToList().Where(x => x.Object == this).ToList().ForEach(x => plan.Details.Remove(x));

            CurrentPlan = plan;
            var period = new Period(plan.StartDate);

            while (period.End() <= plan.EndDate)
            {

                //todo пока-что реализован алгоритм для часов, если учитывать что-то другое, с другим показателем наработки, то нужно вводить понятие плановой наработки и брать её из справочника, либо рассчитывать среднее
                var hours = period.Hours();

                //признак того, что за период работы уже запланированы
                var maintenanceFlag = false;  

                intervals.ForEach(interval =>
                {
                    var type = interval.MaintenanceType.Id;

                    if (!maintenanceFlag)
                    {
                        //проверка по наработке
                        if (interval.MinUsage != null && (lastUsageMap[type].Value + hours) >= interval.MinUsage.Value )
                        {
                            maintenanceFlag = true;

                            //дата проведения обслуживания
                            var date = period.Start().AddDays((interval.MinUsage.Value - lastUsageMap[type].Value) / 24);

                            plan.Details.Add(new MaintenancePlanDetail
                            {
                                Object = this,
                                MaintenanceType = interval.MaintenanceType,
                                Plan = plan,
                                MaintenanceDate = date
                            });

                            //todo здесь не совсем понятно если планировать на последующие периоды, то сколько времени занимает ремонт
                            //может это отражать в каком нибудь справочнике
                            //пока временем ремонта будем пренебрегать

                            intervals.Where(x => x.QuantityInCycle >= interval.QuantityInCycle).ToList().ForEach(x =>
                            {
                                lastUsageMap[x.MaintenanceType.Id] = (int)(period.End() - date).TotalDays * 24;
                                lastDateMap[x.MaintenanceType.Id] = date;
                            });

                        }

                        //проверка по времени
                        else if (interval.PeriodQuantity != null)
                        {
                            //todo сделать проверку интервалов по времени
                        }

                        else
                        {
                            lastUsageMap[type] += hours;
                        }
                    }

                });

                period = period.Next();
                maintenanceFlag = false;
            }
        }
        
        #endregion

    }
}