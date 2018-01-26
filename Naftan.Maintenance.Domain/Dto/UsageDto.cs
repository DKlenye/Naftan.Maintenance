using Naftan.Maintenance.Domain.Objects;
using System;

namespace Naftan.Maintenance.Domain.Dto
{
    public class UsageDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int? PlantId { get; set; }
        public int? DepartmentId { get; set; }
        public string TechIndex { get; set; }
        public OperatingState CurrentOperatingState { get; set; }
        public DateTime? o_date { get; set; }
        public int o_usage { get; set; }
        public DateTime? t_date { get; set; }
        public int t_usage { get; set; }
        public DateTime? s_date { get; set; }
        public int s_usage { get; set; }
        public DateTime? k_date { get; set; }
        public int k_usage { get; set; }
        public int UsageFromStartup { get; set; }
    }
}