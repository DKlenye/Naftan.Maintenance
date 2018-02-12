using System.Collections.Generic;
using Naftan.Common.Domain.EntityComponents;
using Naftan.Maintenance.Domain.Dto;
using Naftan.Maintenance.Domain.Dto.Objects;
using Naftan.Maintenance.Domain.Objects;
using Naftan.Maintenance.Domain.Users;

namespace Naftan.Maintenance.Domain
{
    /// <summary>
    /// Интерфейс фабрики запросов
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// Получить группы оборудования
        /// </summary>
        /// <returns></returns>
        IEnumerable<ObjectGroup> FindObjectGroups();
        /// <summary>
        /// Получить список объектов
        /// </summary>
        /// <returns></returns>
        IEnumerable<ObjectListDto> FindObjects();
        /// <summary>
        /// Получить все данные по оперативному отчёту
        /// </summary>
        /// <returns></returns>
        IEnumerable<OperationalReportDto> FindOperationalReportAll();
        /// <summary>
        /// Получить даные по оперативному отчёту по параметрам выборки
        /// </summary>
        /// <param name="period">период</param>
        /// <param name="userLogin">логин пользователя</param>
        /// <returns></returns>
        IEnumerable<OperationalReportDto> FindOperationalReportByParams(Period period, string userLogin);
        /// <summary>
        /// Получить все данные по графику ППР
        /// </summary>
        /// <returns></returns>
        IEnumerable<MaintenancePlanDto> FindMaintenancePlanAll();
        /// <summary>
        /// Получить данные по графику ППР за период
        /// </summary>
        /// <param name="period">период</param>
        /// <returns></returns>
        IEnumerable<MaintenancePlanDto> FindMaintenancePlanByPeriod(Period period);
        /// <summary>
        /// Получить межремонтные интервалы в развёрнутом виде по (О,Т,С,К)
        /// </summary>
        /// <returns></returns>
        IEnumerable<GroupIntervalDto> FindGroupInterval();
        /// <summary>
        /// Получить наработку с последних ремонтов по оборудованию
        /// </summary>
        /// <returns></returns>
        IEnumerable<UsageDto> FindUsage();
        /// <summary>
        /// Получить оперативный отчёт по id оборудования
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        OperationalReportDto FindOperationalReportByObjectId(int objectId);
        /// <summary>
        /// Получить тех. характеристики оборудования
        /// </summary>
        /// <param name="specificationId">id требуемых характеристик</param>
        /// <returns></returns>
        Dictionary<int, Dictionary<int, string>> FindObjectSpecifications(int[] specificationId);
        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        /// <param name="login">логин пользователя</param>
        /// <returns></returns>
        User FindUserByLogin(string login);
    }
}