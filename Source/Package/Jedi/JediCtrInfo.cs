using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediCtrInfo
    {
        public ConstructorInfo Info { get; }

        public JediCtrInfo(ConstructorInfo info)
        {
            Info = info;
        }

        public object Spawn()
        {
            return null;
        }
    }
}
