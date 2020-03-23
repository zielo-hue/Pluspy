using Pluspy.Core;
using Pluspy.Net;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Pluspy.Utilities
{
    public static class PacketFactory<TPacket> where TPacket : IPacket
    {
        private static Func<JsonSerializable<TPacket>, TPacket> _func;
        private static readonly object _lock = new object();

        public static TPacket FromModel(JsonSerializable<TPacket> model)
        {
            if (_func is null)
            {
                lock (_lock)
                {
                    var modelType = model.GetType();
                    var cast = Expression.Convert(Expression.Constant(model), modelType);
                    var constructor = typeof(TPacket).GetConstructor(new[] { modelType });

                    if (constructor is null)
                        throw new InvalidOperationException($"The packet must have a public constructor that accepts {modelType.Name} as the only argument.");

                    var @new = Expression.New(constructor, cast);
                    var parameter = Expression.Parameter(typeof(JsonSerializable<TPacket>));
                    var lambda = Expression.Lambda<Func<JsonSerializable<TPacket>, TPacket>>(@new, parameter);
                    _func = lambda.Compile();
                }
            }

            return _func(model);
        }
    }

    public static class PacketFactory
    {
        private static readonly Dictionary<Type, Func<IPacket>> _dict = new Dictionary<Type, Func<IPacket>>();
        public static IPacket CreateInstance(Type type)
        {
            if (_dict.TryGetValue(type, out var func))
                return func();

            var newFunc = Expression.Lambda<Func<IPacket>>(Expression.Convert(Expression.New(type), typeof(IPacket))).Compile();

            _dict.Add(type, newFunc);
            return newFunc();
        }
    }
}
