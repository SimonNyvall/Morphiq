using System;

namespace Morphiq.Attributes
{
    public class MorphPropertyNameAttribute : Attribute
    {
        public string TargetName { get; }

        public MorphPropertyNameAttribute(string targetName)
        {
            TargetName = targetName;
        }
    }    
}

