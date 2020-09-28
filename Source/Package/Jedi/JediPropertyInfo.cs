using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediPropertyInfo
    {
        public PropertyInfo Info { get; }

        public JediPropertyInfo(PropertyInfo info)
        {
            Info = info;
        }

        public void Set(object value)
        { 
        }
    }
}
