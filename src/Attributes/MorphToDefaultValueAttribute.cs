using System;

namespace Morphiq.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MorphToDefaultValueAttribute : Attribute
    {
        public object DefaultValue { get; }

        public MorphToDefaultValueAttribute(object defaultValue)
        {
            DefaultValue = defaultValue;
        }
    }
}