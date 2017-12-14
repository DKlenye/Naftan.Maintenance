using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;
using Newtonsoft.Json;
using System.Linq;

namespace Naftan.Maintenance.WebApplication.DependencyInjection
{
    public class NHibernateContractResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            if (typeof(INHibernateProxy).IsAssignableFrom(objectType))
                return base.CreateContract(objectType.BaseType);
            else
                return base.CreateContract(objectType);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {

            var z = base.CreateProperties(type, memberSerialization);
            var y = z.Where(x =>
            {

                if (type.GetField(x.PropertyName) != null) return true;
                
                var isPrimitive = x.PropertyType.IsPrimitive;
                var prop = type.GetProperty(x.PropertyName);
                var isVirtual = prop.GetGetMethod().IsVirtual;
                return isPrimitive || !isVirtual;
            }).ToList();

            return y;
        }

        protected override JsonProperty CreateProperty(
            MemberInfo member,
            MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasPrivateSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasPrivateSetter;
                }
            }

            return prop;
        }

    }
}
