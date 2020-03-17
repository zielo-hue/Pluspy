using System;

namespace Pluspy.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PropertyAttribute : Attribute
    {
        public string Name { get; }

        public PropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
