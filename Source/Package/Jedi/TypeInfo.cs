using System;
using System.Linq;

namespace Jedi
{
    public class TypeInfo
    {
        /// <summary>
        /// The underlying type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// If the type is disposable
        /// </summary>
        bool IsDisposable { get; }

        public JediCtrInfo Ctr { get; }

        public JediFieldInfo[] Fields { get; }

        public JediPropertyInfo[] Properties { get;  }

        public JediMethodInfo[] Methods { get;  }

        public TypeInfo(Type type)
        {
            Type = type;
            IsDisposable = Type.GetInterfaces().Contains(typeof(IDisposable));
        }
    }
}
