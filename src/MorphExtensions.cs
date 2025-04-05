using System;
using System.Linq;
using System.Reflection;
using Morphiq.Attributes;

namespace Morphiq
{
    public static class MorphExtensions
    {
        public static T MorphTo<T>(this object fromObject, Action<T> configuration = null) where T : class, new()
        {
            var toObject = new T();
            var fromProperties = fromObject.GetType().GetProperties();
            var toProperties = typeof(T).GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                var targetName = fromProperty.Name;
                var attribute = fromProperty.GetCustomAttribute<MorphPropertyNameAttribute>();

                if (attribute != null && !string.IsNullOrEmpty(attribute.TargetName))
                {
                    targetName = attribute.TargetName;
                }

                var toProperty = toProperties.FirstOrDefault(x => x.Name == targetName);

                if (toProperty != null && toProperty.CanWrite)
                {
                    toProperty.SetValue(toObject, fromProperty.GetValue(fromObject));
                }
            }
            
            configuration?.Invoke((toObject));

            return toObject;
        }
    }
}

