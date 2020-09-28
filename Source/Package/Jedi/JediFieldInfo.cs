using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediFieldInfo
    {
        public FieldInfo Info { get; }

        public object Id { get; }

        public object Optional { get; }

        public JediFieldInfo(FieldInfo info)
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
