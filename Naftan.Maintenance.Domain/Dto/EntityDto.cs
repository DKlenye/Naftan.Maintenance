using Naftan.Common.Domain;

namespace Naftan.Maintenance.Domain.Dto
{
    public abstract class EntityDto<TEntity>
    {
        public int Id { get; set; }

        public abstract void Merge(TEntity entity, IRepository repository);
        public abstract TEntity GetEntity(IRepository repository);
        public abstract void SetEntity(TEntity entity);
               

    }
}
