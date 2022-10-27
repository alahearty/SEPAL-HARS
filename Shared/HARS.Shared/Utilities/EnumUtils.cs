using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Utilities
{
    public static class EnumUtils
    {
        public static string GetDescription(this Enum value)
        {
            var descriptionAttribute = value.GetAttribute<DescriptionAttribute>();
            return descriptionAttribute == null ? value.ToString() : descriptionAttribute.Description;
        }


        private static T? GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

        public static T GetValueFromDescription<T>(this string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            return default;
        }

        public static IReadOnlyList<T> GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
