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
        private static ConcurrentDictionary<Type, TypeInfo> _registry;

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
