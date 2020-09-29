using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    public static class Registry
    {
        /// <summary>
        /// A mapping of all known types of Jedi
        /// </summary>
        private static ConcurrentDictionary<Type, TypeInfo> _registry = new ConcurrentDictionary<Type, TypeInfo>();

        /// <summary>
        /// How injection occurrs
        /// Reflection - Using regular reflection
        /// Compiled - Set reflection methods are compiled into 
        /// delegates which makes it as fast as a native method call.
        /// </summary>
        public static InjectMechanism InjectMechanism { get; set; } = InjectMechanism.Compiled;

        /// <summary>
        /// Register the C# primitive types by default
        /// </summary>
        static Registry()
        {
            Register(typeof(bool));
            Register(typeof(byte));
            Register(typeof(sbyte));
            Register(typeof(char));
            Register(typeof(decimal));
            Register(typeof(double));
            Register(typeof(float));
            Register(typeof(int));
            Register(typeof(uint));
            Register(typeof(long));
            Register(typeof(ulong));
            Register(typeof(short));
            Register(typeof(ushort));
            Register(typeof(string));
        }

        /// <summary>
        /// Register a given type in the Jedi Registry 
        /// </summary>
        /// <param name="type"></param>
        public static void Register(Type type)
        {
            _registry.TryAdd(type, new TypeInfo(type));
        }

        /// <summary>
        /// Obtain a registered type info
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeInfo GetTypeInfo(Type type)
        {
            // Check if the type is already registered
            if (_registry.ContainsKey(type) == false)
            {
                Register(type);
            }

            // Return the type
            return _registry[type];
        }
    }
}
