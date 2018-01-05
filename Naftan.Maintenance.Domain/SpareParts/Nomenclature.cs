using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.SpareParts
{
    public class Nomenclature:TreeNode<Nomenclature>,IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
