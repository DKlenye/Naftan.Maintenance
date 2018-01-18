using System.ComponentModel;

namespace Naftan.Maintenance.Domain.Objects
{
    /// <summary>
    /// Вид эксплуатации (рабочее состояние)
    /// </summary>
    public enum OperatingState
    {
        /// <summary>
        /// Эксплуатируется
        /// </summary>
        [Description("Эксплуатируется")] Operating = 1,
        /// <summary>
        /// На обслуживании
        /// </summary>
        [Description("На обслуживании")] Maintenance = 2,
        /// <summary>
        /// Резерв
        /// </summary>
        [Description("Резерв")] Reserve = 3,
        /// <summary>
        /// Списан
        /// </summary>
        [Description("Не эксплуатируется")] WriteOff = 4
    }
}