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
                targetName = HandleMorphToProperty<T>(fromProperty, targetName);

                var toProperty = toProperties.FirstOrDefault(x => x.Name == targetName);

                if (toProperty == null || !toProperty.CanWrite) continue;
                
                if (ShouldIgnoreProperty(fromProperty, toProperty)) continue;

                var customMappingAttribute = fromProperty.GetCustomAttribute<MorphToCustomMappingAttribute>() ??
                                             toProperty.GetCustomAttribute<MorphToCustomMappingAttribute>();
                
                if (HandleCustomMapping(fromObject, customMappingAttribute, fromProperty, toProperty, toObject)) continue;

                var valueToSet = fromProperty.GetValue(fromObject);
                valueToSet = HandleDefaultValue<T>(valueToSet, fromProperty, toProperty);

                toProperty.SetValue(toObject, valueToSet);
            }

            configuration?.Invoke(toObject);

            return toObject;
        }

        private static object HandleDefaultValue<T>(object valueToSet, PropertyInfo fromProperty, PropertyInfo toProperty)
            where T : class, new()
        {
            if (valueToSet != null) return valueToSet;
            
            var defaultValueAttribute = fromProperty.GetCustomAttribute<MorphToDefaultValueAttribute>() ??
                                        toProperty.GetCustomAttribute<MorphToDefaultValueAttribute>();
            if (defaultValueAttribute != null)
            {
                valueToSet = defaultValueAttribute.DefaultValue;
            }

            return valueToSet;
        }

        private static string HandleMorphToProperty<T>(PropertyInfo fromProperty, string targetName) where T : class, new()
        {
            var morphToPropertyAttribute = fromProperty.GetCustomAttribute<MorphToPropertyAttribute>();

            if (morphToPropertyAttribute != null && !string.IsNullOrEmpty(morphToPropertyAttribute.TargetName))
            {
                targetName = morphToPropertyAttribute.TargetName;
            }

            return targetName;
        }

        private static bool HandleCustomMapping<T>(object fromObject, MorphToCustomMappingAttribute customMappingAttribute,
            PropertyInfo fromProperty, PropertyInfo toProperty, T toObject) where T : class, new()
        {
            if (customMappingAttribute == null) return false;
            
            var method = fromProperty.DeclaringType?.GetMethod(customMappingAttribute.MappingMethodName,
                             BindingFlags.NonPublic | BindingFlags.Instance) ??
                         toProperty.DeclaringType?.GetMethod(customMappingAttribute.MappingMethodName,
                             BindingFlags.NonPublic | BindingFlags.Instance);

            if (method == null) return false;
                
            var targetInstance = method.DeclaringType == fromProperty.DeclaringType ? fromObject : toObject;
            var value = method.Invoke(targetInstance, new[] { fromProperty.GetValue(fromObject) });
            toProperty.SetValue(toObject, value);
            
            return true;
        }

        private static bool ShouldIgnoreProperty(PropertyInfo fromProperty, PropertyInfo toProperty)
        {
            return fromProperty.GetCustomAttribute<IgnorePropertyAttribute>() != null ||
                   toProperty.GetCustomAttribute<IgnorePropertyAttribute>() != null;
        }
    }
}