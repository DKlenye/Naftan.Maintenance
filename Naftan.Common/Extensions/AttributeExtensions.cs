using System;
using System.Linq;
using System.Reflection;

namespace Naftan.Common.Extensions
{
    public static class AttributeExtensions
    {
        public static T[] GetAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit = true)
            where T : Attribute
        {
            return (T[]) attributeProvider.GetCustomAttributes(typeof (T), inherit);
        }

        public static T GetAttribute<T>(this ICustomAttributeProvider attributeProvider, bool inherit = true)
            where T : Attribute
        {
            return attributeProvider.GetAttributes<T>(inherit).SingleOrDefault();
        }

        public static bool HasAttribute<T>(this ICustomAttributeProvider attributeProvider, bool inherit = true)
            where T : Attribute
        {
            return attributeProvider.GetAttributes<T>(inherit).Any();
        }
    }
}
