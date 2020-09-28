using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public class JediMethodInfo
    {
        public MethodInfo Info { get; }

        public ParameterInfo[] Parameters { get; }

        public JediMethodInfo(MethodInfo info)
        {
            Info = info;
            Parameters = Info.GetParameters();
        }

        public void Invoke(object[] parameters)
        { 
        }

        public async Task InvokeAsync(object[] parameters)
        { 
        }
    }
}
