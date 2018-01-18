using System;

namespace Naftan.Maintenance.Domain.Dto.Objects
{
    public class MaintenancePlanDto
    {
       public int Id { get; set; }
	   public int ObjectId { get; set; }
	   public int GroupId { get; set; }
	   public string TechIndex { get; set; }
	   public int? DepartmentId { get; set; }
	   public int? PlantId { get; set; }
	   public DateTime MaintenanceDate { get; set; }
	   public int MaintenanceTypeId { get; set; }
	   public int MaintenanceReasonId { get; set; }
    }
}
