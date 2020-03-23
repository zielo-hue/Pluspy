using System;

namespace Pluspy.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute
    {
        public Type PacketType { get; }

        public PacketHandlerAttribute(Type type)
        {
            PacketType = type;
        }
    }
}
