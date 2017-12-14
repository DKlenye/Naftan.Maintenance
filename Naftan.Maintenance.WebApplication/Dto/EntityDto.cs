using Naftan.Common.Domain;

namespace Naftan.Maintenance.WebApplication.Dto
{
    public abstract class EntityDto<TEntity>
        where TEntity:IEntity
    {
        public int Id { get; set; }

        protected EntityDto(TEntity entity) {
            Wrap(entity);
        }

        protected abstract void Wrap(TEntity entity);
        public abstract TEntity UnWrap();
        public abstract void Merge(TEntity entity);
    }
}