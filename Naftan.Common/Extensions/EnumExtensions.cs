using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Naftan.Common.Extensions
{
    public static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            return memberInfo[0].GetAttribute<T>();
        }

        public static string ToDescription(this Enum value)
        {
            var attribute = value.GetAttribute<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static IEnumerable<string> ToDescriptions(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return
                type.GetEnumNames()
                    .Select(value => Enum.Parse(type, value))
                    .Select(e => (e as Enum).ToDescription())
                    .ToList();
        }

        public static Dictionary<int, string> ToDictionary(this Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Enum.GetValues(type)
                .Cast<Enum>()
                .Select(x => new KeyValuePair<int, string>(int.Parse(x.ToString("D")), x.ToDescription()))
                .ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
