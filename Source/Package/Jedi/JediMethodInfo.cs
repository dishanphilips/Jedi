using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public class JediMethodInfo
    {
        /// <summary>
        /// The method information
        /// </summary>
        public MethodInfo Info { get; }

        /// <summary>
        /// The required parameters for the method
        /// </summary>
        public Type[] Parameters { get; }

        /// <summary>
        /// Create Jedi Method for a given MethodInfo
        /// </summary>
        /// <param name="info"></param>
        public JediMethodInfo(MethodInfo info)
        {
            // Set the data
            Info = info;
            Parameters = Info.GetParameters().Select(p => p.ParameterType).ToArray();

            // TODO : Precomile the method to a delegate
        }

        /// <summary>
        /// Invoke a method on a given target with the given targets
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parameters"></param>
        public void Invoke(object target, object[] parameters)
        {
            Info.Invoke(target, parameters);
        }
    }
}
