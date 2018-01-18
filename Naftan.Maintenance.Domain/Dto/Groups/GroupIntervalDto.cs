namespace Naftan.Maintenance.Domain.Dto.Groups
{
    public class GroupIntervalDto
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public int? O_min { get; set; }
        public int? O_max { get; set; }
        public int? O_qt { get; set; }
        public int? T_min { get; set; }
        public int? T_max { get; set; }
        public int? T_qt { get; set; }
        public int? S_min { get; set; }
        public int? S_max { get; set; }
        public int? S_qt { get; set; }
        public int? K_min { get; set; }
        public int? K_max { get; set; }
        public int? K_qt { get; set; }

    }
}
