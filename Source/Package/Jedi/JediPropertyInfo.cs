using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediPropertyInfo
    {
        public PropertyInfo Info { get; }

        public object Id { get; }

        public object Optional { get; }

        public JediPropertyInfo(PropertyInfo info)
        {
            // Get the attribute
            InjectAttribute injectAttribute = info.GetCustomAttribute<InjectAttribute>();

            // Set the data
            Info = info;
            Id = injectAttribute.Id;
            Optional = injectAttribute.Optional;
        }

        public void Set(object value)
        { 
        }
    }
}
