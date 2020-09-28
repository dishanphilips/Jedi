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

        public object Id { get; }

        public object Optional { get; }

        public JediMethodInfo(MethodInfo info)
        {
            // Get the attribute
            InjectAttribute injectAttribute = info.GetCustomAttribute<InjectAttribute>();

            // Set the data
            Info = info;
            Parameters = Info.GetParameters();
            Id = injectAttribute.Id;
            Optional = injectAttribute.Optional;
        }

        public void Invoke(object[] parameters)
        { 
        }

        public async Task InvokeAsync(object[] parameters)
        { 
        }
    }
}
