using Newtonsoft.Json;
using System.Collections.Generic;

namespace Naftan.Common.Domain
{
    /// <summary>
    /// Древовидная структура данных, хранящая связи предков и потомков
    /// </summary>
    /// <typeparam name="T">Тип узла</typeparam>
    public abstract class TreeNode<T>
        where T : TreeNode<T>
    {
      
        private readonly ICollection<T> ancestors = new HashSet<T>();
        private readonly ICollection<T> children = new HashSet<T>();
        private readonly ICollection<T> descendants = new HashSet<T>();

        /// <summary>
        /// Родитель текущего узла
        /// </summary>
        [JsonIgnore]
        public virtual T Parent { get; private set; }

        /// <summary>
        /// Дети текущего узла
        /// </summary>
        
       [JsonProperty("data")]
        public virtual IEnumerable<T> Children => children;

        /// <summary>
        ///  Узлы, которые находятся над текущим узлом (родители, прородители и т.д. )
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<T> Ancestors => ancestors;

        /// <summary>
        /// Узлы, которые находятся под текущим узлом (дети, внуки и т.д.)   
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<T> Descendants => descendants;

        /// <summary>
        /// 
        /// </summary>
        protected T This => (T)this;

        /// <summary>
        /// Добавить ребёнка
        /// </summary>
        /// <param name="child"></param>
        public virtual void AddChild(T child)
        {
            children.Add(child);
            child.Parent = This;

            SetAncestorDescendantRelation(This, child);
        }

        /// <summary>
        /// Очистить родителя
        /// </summary>
        public virtual void ClearParent()
        {
            if (Parent == null)
                return;

            UnSetAncestorDescendantRelation(Parent, This);
            var collection = (ICollection<T>)Parent.Children;
            collection.Remove(This);
            Parent = null;
        }

        /// <summary>
        /// Убрать связь между узлами
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="descendant"></param>
        private static void UnSetAncestorDescendantRelation(T ancestor, T descendant)
        {
            ChangeAncestorDescendantRelation(ancestor, descendant, false);
        }

        /// <summary>
        /// Добавить связь между узлами
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="descendant"></param>
        private static void SetAncestorDescendantRelation(T ancestor, T descendant)
        {
            ChangeAncestorDescendantRelation(ancestor, descendant, true);
        }

        /// <summary>
        /// Поменять связь между узлами
        /// </summary>
        /// <param name="ancestor">предок</param>
        /// <param name="descendant">потомок</param>
        /// <param name="addRelation">добавить/убрать связь</param>
        private static void ChangeAncestorDescendantRelation(T ancestor, T descendant, bool addRelation)
        {
            if (ancestor.Parent != null)
                ChangeAncestorDescendantRelation(ancestor.Parent, descendant, addRelation);

            foreach (T grandDescendant in descendant.children)
                ChangeAncestorDescendantRelation(ancestor, grandDescendant, addRelation);

            var ancestorDescendants = (ICollection<T>)ancestor.Descendants;
            var descendantAncestors = (ICollection<T>)descendant.Ancestors;

            if (addRelation)
            {
                ancestorDescendants.Add(descendant);
                descendantAncestors.Add(ancestor);
            }
            else
            {
                ancestorDescendants.Remove(descendant);
                descendantAncestors.Remove(ancestor);
            }
        }
    }
}
