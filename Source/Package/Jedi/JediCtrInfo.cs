using System;
using System.Linq;
using System.Reflection;

namespace Jedi
{
    public class JediCtrInfo
    {
        /// <summary>
        /// The reflection constructor info
        /// </summary>
        public ConstructorInfo Info { get; }

        /// <summary>
        /// The required parameters for the constructor
        /// </summary>
        public Type[] Parameters { get; }

        /// <summary>
        /// Construct a Jedi Ctr Info with a given Reflection Constructor Info
        /// </summary>
        /// <param name="info"></param>
        public JediCtrInfo(ConstructorInfo info)
        {
            Info = info;
            Parameters = Info.GetParameters().Select(p => p.ParameterType).ToArray();

            // TODO : Precomile the constructor to a delegate
        }

        /// <summary>
        /// Spawn an instance with the given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Spawn(object[] parameters)
        {
            return Info.Invoke(parameters);
        }
    }
}
