using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HARS.Shared.Extensions
{
    public static class ObjectExtension
    {
        public static T DeepClone<T>(this T obj, string[] ignore, params object[] contructorMembers) where T : class
        {
            var newObject = Activator.CreateInstance(typeof(T), contructorMembers);

            var properties = newObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (ignore.NotContains(property.Name))
                    property.SetValue(newObject, property.GetValue(obj));
            }

            return (T)newObject;
        }
        public static T Clone<T>(this T obj) where T : class
        {
            if (obj is null)
            {
                return default;
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj, serializeSettings), deserializeSettings);
        }
    }
}
