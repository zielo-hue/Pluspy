using Pluspy.Enums;
using System;

namespace Pluspy.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class Serverbound : Attribute
    {
        public State State { get; }

        public Serverbound(State state)
        {
            State = state;
        }
    }
}
