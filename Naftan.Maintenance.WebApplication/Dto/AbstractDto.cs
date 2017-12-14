using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public abstract class AbstractDto<TEntity>
        where TEntity : IEntity
    {
        public int Id { get; set; }

        public abstract void Merge(TEntity entity);
        public abstract TEntity GetEntity();
        public abstract void SetEntity(TEntity entity);
    }
}