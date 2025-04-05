using System;

namespace Morphiq.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MorphToCustomMappingAttribute : Attribute
    {
        public string MappingMethodName { get; }

        public MorphToCustomMappingAttribute(string mappingMethodName)
        {
            MappingMethodName = mappingMethodName;
        }
    }
}