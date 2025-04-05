using System;

namespace Morphiq.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MorphToPropertyAttribute : Attribute
    {
        public string TargetName { get; }

        public MorphToPropertyAttribute(string targetName)
        {
            TargetName = targetName;
        }
    }    
}

