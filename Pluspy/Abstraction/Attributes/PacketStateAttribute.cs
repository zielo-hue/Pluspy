using Pluspy.Enums;
using System;

namespace Pluspy.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class PacketStateAttribute : Attribute
    {
        public State State { get; }

        public PacketStateAttribute(State state)
        {
            State = state;
        }
    }
}
