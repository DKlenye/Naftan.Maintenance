using System;
using System.Collections.Generic;
using System.Linq;

namespace Naftan.Common.Domain.Impl
{
   /// <summary>
   /// Реализация репозитория в памяти
   /// </summary>
   public  class InMemoryRepository:IRepository
   {
       private readonly Dictionary<Type, Dictionary<int, object>> Data = new Dictionary<Type, Dictionary<int, object>>();
       private readonly Dictionary<Type, int> Id = new Dictionary<Type, int>(); 

       private Dictionary<int, object> GetList<T>()
       {
           Dictionary<int, object> dictionary;

           if (Data.TryGetValue(typeof (T), out dictionary))
           {
               return dictionary;
           }

           dictionary = new Dictionary<int, object>();
           Data.Add(typeof (T), dictionary);
           Id.Add(typeof (T), 1);
           return dictionary;
       }

       private int GenerateId<T>()
       {
           return Id[typeof (T)]++;
       }

       public IEnumerable<TEntity> All<TEntity>() where TEntity : IEntity
       {
           return GetList<TEntity>().Values.OfType<TEntity>();
       }

       public TEntity Get<TEntity>(int id) where TEntity : IEntity
       {
           var list = GetList<TEntity>(); 

           if (list.ContainsKey(id))
           {
               return (TEntity) list[id];
           }

           throw new KeyNotFoundException("Сущность с указаным id не найдена");
       }

       public void Save<TEntity>(TEntity entity) where TEntity : IEntity
       {
           var list = GetList<TEntity>();

           if (entity.Id == 0)
           {
               entity.Id = GenerateId<TEntity>();
               list.Add(entity.Id, entity);
           }
           else
           {
               list[entity.Id] = entity;
           }

       }

       public void Remove<TEntity>(TEntity entity) where TEntity : IEntity
       {
           if (entity.Id != 0)
           {
                Remove<TEntity>(entity.Id); 
           }
       }

        public void Remove<TEntity>(int id) where TEntity : IEntity
        {
            if (id != 0)
            {
                var list = GetList<TEntity>();
                list.Remove(id);
            }
        }
    }
}
