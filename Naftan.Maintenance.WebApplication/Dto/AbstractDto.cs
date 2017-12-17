using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public abstract class AbstractDto<TEntity>
        where TEntity : IEntity
    {
        public int Id { get; set; }

        public abstract void Merge(TEntity entity,IRepository repository);
        public abstract TEntity GetEntity(IRepository repository);
        public abstract void SetEntity(TEntity entity);

    }
}