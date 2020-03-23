using Pluspy.Enums;
using System;

namespace Pluspy.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public sealed class Clientbound : Attribute
    {
        public State State { get; }

        public Clientbound(State state)
        {
            State = state;
        }
    }
}
