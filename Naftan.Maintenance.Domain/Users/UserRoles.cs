using System.ComponentModel;

namespace Naftan.Maintenance.Domain.Users
{
    /// <summary>
    /// Роли пользователей
    /// </summary>
    public enum UserRoles
    {
        /// <summary>
        /// Администратор
        /// </summary>
        [Description("Администратор")] Maintenance_Admins,

        /// <summary>
        /// Пользователь
        /// </summary>
        [Description("Пользователь")] Maintenance_Users,

        /// <summary>
        /// Инженер
        /// </summary>
        [Description("Инженер")] Maintenance_Engineer,

        /// <summary>
        /// Механик
        /// </summary>
        [Description("Механик")] Maintenance_Mechanic,

        /// <summary>
        /// Электрик
        /// </summary>
        [Description("Электрик")] Maintenance_Electric

    }
}
