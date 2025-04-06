﻿using System;
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
                var morphToPropertyAttribute = fromProperty.GetCustomAttribute<MorphToPropertyAttribute>();

                if (morphToPropertyAttribute != null && !string.IsNullOrEmpty(morphToPropertyAttribute.TargetName))
                {
                    targetName = morphToPropertyAttribute.TargetName;
                }

                var toProperty = toProperties.FirstOrDefault(x => x.Name == targetName);

                if (toProperty != null && toProperty.CanWrite)
                {
                    if (fromProperty.GetCustomAttribute<IgnorePropertyAttribute>() != null ||
                        toProperty.GetCustomAttribute<IgnorePropertyAttribute>() != null)
                    {
                        continue;
                    }

                    var customMappingAttribute = fromProperty.GetCustomAttribute<MorphToCustomMappingAttribute>() ??
                                                 toProperty.GetCustomAttribute<MorphToCustomMappingAttribute>();
                    if (customMappingAttribute != null)
                    {
                        var method = fromProperty.DeclaringType.GetMethod(customMappingAttribute.MappingMethodName,
                            BindingFlags.NonPublic | BindingFlags.Instance) ??
                                     toProperty.DeclaringType.GetMethod(customMappingAttribute.MappingMethodName,
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        if (method != null)
                        {
                            var targetInstance = method.DeclaringType == fromProperty.DeclaringType ? fromObject : toObject;
                            var value = method.Invoke(targetInstance, new[] { fromProperty.GetValue(fromObject) });
                            toProperty.SetValue(toObject, value);
                            continue;
                        }
                    }

                    var valueToSet = fromProperty.GetValue(fromObject);
                    if (valueToSet == null)
                    {
                        var defaultValueAttribute = fromProperty.GetCustomAttribute<MorphToDefaultValueAttribute>() ??
                                                    toProperty.GetCustomAttribute<MorphToDefaultValueAttribute>();
                        if (defaultValueAttribute != null)
                        {
                            valueToSet = defaultValueAttribute.DefaultValue;
                        }
                    }

                    toProperty.SetValue(toObject, valueToSet);
                }
            }

            configuration?.Invoke(toObject);

            return toObject;
        }
    }
}