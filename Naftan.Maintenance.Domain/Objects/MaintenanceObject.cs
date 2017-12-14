using System;
using System.Collections.Generic;
using System.Linq;
using Naftan.Maintenance.Domain.Specifications;
using Naftan.Common.Domain;
using Naftan.Maintenance.Domain.ObjectMaintenance;
using Naftan.Maintenance.Domain.Usage;

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

        public MaintenanceObject(ObjectGroup group, string techIndex, DateTime startOperating)
        {
            Group = group;
            TechIndex = techIndex;
            StartOperating = startOperating;
            ChangeOperatingState(OperatingState.Operating, startOperating);
        }

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
        public Environment Environment { get; set; }
        /// <summary>
        /// Завод производитель
        /// </summary>
        public Manufacturer Manufacturer { get; set; }
        /// <summary>
        ///Дата изготовления 
        /// </summary>
        public DateTime? ManufactureDate { get; set; }
        /// <summary>
        /// Заводской номер
        /// </summary>
        public string FactoryNumber { get; set; }
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
            UsageFromStartup  = UsageFromStartup??0+ usage;
        }

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
                Type = maintenanceType,
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
                var targetInterval = Intervals.SingleOrDefault(x => x.MaintenanceType == CurrentMaintenance.Type);


                /* Выбрать интервалы для сброса. Сюда попадает интересующий нас интервал и более мелкие.
                 * Т.е. Если проводится Средний ремонт, то сбрасываются Средний, Текущий и Обслуживание.
                */

                //todo В данном случае выбираетя более мелкий интервал по количеству в ремонтном цикле,а если количества нет?
                var intervalsForReset = Intervals.Where(
                    x => x.QuantityInCycle >= targetInterval.QuantityInCycle)
                    .ToDictionary(i => i.MaintenanceType);

                //Если последнего обслуживания нет, то добавляем его
                if (lastMaintenance.All(x => x.MaintenanceType != CurrentMaintenance.Type))
                {
                    var newLastMaintenance = new LastMaintenance(
                            CurrentMaintenance.Type,
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

        /// <summary>
        /// Наработка с начала экслуатации
        /// </summary>
        public int? UsageFromStartup { get; private set; }
    }
}