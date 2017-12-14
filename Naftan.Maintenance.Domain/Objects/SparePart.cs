using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Objects
{
    public class SparePart:IEntity
    {
        public int Id { get; set; }
        public Nomenclature Nomenclature { get; set; }
        public MaintenanceObject Object { get; set; }
        public MeasureUnit Unit { get; set; }
        public decimal Quantity { get; set; }
    }
}