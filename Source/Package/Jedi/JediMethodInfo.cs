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
        /// Compiled set action for 0 argument method
        /// </summary>
        public Action<object> Method0 { get; }

        /// <summary>
        /// Compiled set action for 1 argument method
        /// </summary>
        public Action<object, object> Method1 { get; }

        /// <summary>
        /// Compiled set action for 2 argument method
        /// </summary>
        public Action<object, object, object> Method2 { get; }

        /// <summary>
        /// Compiled set action for 3 argument method
        /// </summary>
        public Action<object, object, object, object> Method3 { get; }

        /// <summary>
        /// Compiled set action for 4 argument method
        /// </summary>
        public Action<object, object, object, object, object> Method4 { get; }

        /// <summary>
        /// Compiled set action for 5 argument method
        /// </summary>
        public Action<object, object, object, object, object, object> Method5 { get; }

        /// <summary>
        /// Create Jedi Method for a given MethodInfo
        /// </summary>
        /// <param name="info"></param>
        public JediMethodInfo(MethodInfo info)
        {
            // Set the data
            Info = info;
            Parameters = Info.GetParameters().Select(p => p.ParameterType).ToArray();

            // Compile the delegate if required
            if (Registry.InjectMechanism == InjectMechanism.Compiled)
            {
                switch (Parameters.Length)
                {
                    case 0:
                        Method0 = Info.CreateDelegate(typeof(Action<object>)) 
                            as Action<object>;
                        break;
                    case 1:
                        Method1 = Info.CreateDelegate(typeof(Action<object, object>)) 
                            as Action<object, object>;
                        break;
                    case 2:
                        Method2 = Info.CreateDelegate(typeof(Action<object, object, object>)) 
                            as Action<object, object, object>;
                        break;
                    case 3:
                        Method3 = Info.CreateDelegate(typeof(Action<object, object, object, object>)) 
                            as Action<object, object, object, object>;
                        break;
                    case 4:
                        Method4 = Info.CreateDelegate(typeof(Action<object, object, object, object, object>)) 
                            as Action<object, object, object, object, object>;
                        break;
                    case 5:
                        Method5 = Info.CreateDelegate(typeof(Action<object, object, object, object, object, object>)) 
                            as Action<object, object, object, object, object, object>;
                        break;
                    default:
                        throw new Exception("Only a  maximum of 5 parameters are currently supported!");
                }
            }
        }

        /// <summary>
        /// Invoke a method on a given target with the given targets
        /// </summary>
        /// <param name="target"></param>
        /// <param name="parameters"></param>
        public void Invoke(object target, object[] parameters)
        {
            switch (Registry.InjectMechanism)
            {
                case InjectMechanism.Compiled:
                    // Invoke the method according to the number of parameters
                    switch(Parameters.Length)
                    {
                        case 0:
                            Method0.Invoke(target);
                            break;
                        case 1:
                            Method1.Invoke(target, parameters[0]);
                            break;
                        case 2:
                            Method2.Invoke(target, parameters[0], parameters[1]);
                            break;
                        case 3:
                            Method3.Invoke(target, parameters[0], parameters[1], parameters[2]);
                            break;
                        case 4:
                            Method4.Invoke(target, parameters[0], parameters[1], parameters[2], parameters[3]);
                            break;
                        case 5:
                            Method5.Invoke(target, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                            break;
                    }
                    break;
                case InjectMechanism.Reflection:
                    Info.Invoke(target, parameters);
                    break;
            }            
        }
    }
}
