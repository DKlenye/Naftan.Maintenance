using System.Collections.Generic;

namespace Naftan.Common.Domain
{
    /// <summary>
    /// Интерфейс репозитория
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Получить список всех сущностей
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <returns>Список сущностей</returns>
        IEnumerable<TEntity> All<TEntity>() where TEntity:IEntity;

        /// <summary>
        /// Получить сущность по идентификатору.
        /// </summary> 
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="id"> Идектификатор сущности </param>
        /// <returns> Сущность с указанным Id, если существует. Иначе - null. </returns>
        TEntity Get<TEntity>(int id) where TEntity:IEntity;

        /// <summary>
        /// Сохранить сущность
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity"> Сущность </param>
        void Save<TEntity>(TEntity entity) where TEntity : IEntity;

        /// <summary>
        /// Удалить сущность
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity"> Сущность </param>
        void Remove<TEntity>(TEntity entity)  where TEntity:IEntity;

        /// <summary>
        /// Удалить сущность по идентификатору
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="entity"> Сущность </param>
        void Remove<TEntity>(int id) where TEntity : IEntity;

    }
}