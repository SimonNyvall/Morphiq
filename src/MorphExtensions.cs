using System.Linq;
using System.Reflection;
using Morphiq.Attributes;

namespace Morphiq
{
    public static class MorphExtensions
    {
        public static ToType MorphTo<ToType>(this object fromObj) where ToType : new()
        {
            var toObject = new ToType();
            var fromProperties = fromObj.GetType().GetProperties();
            var toProperties = typeof(ToType).GetProperties();

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
                    toProperty.SetValue(toObject, fromProperty.GetValue(fromObj));
                }
            }

            return toObject;
        }
    }
}

