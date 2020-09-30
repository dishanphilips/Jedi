using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Jedi
{
    /// <summary>
    /// The reason why compiled contstructors are preffered
    /// |               Method |       Mean |     Error |    StdDev |     Median |  Gen 0 | Allocated |
    /// |--------------------- |-----------:|----------:|----------:|-----------:|-------:|----------:|
    /// |         ExplicitCtor |   2.591 ns | 0.0523 ns | 0.0489 ns |   2.582 ns | 0.0076 |      12 B |
    /// |        WithActivator | 113.661 ns | 0.4652 ns | 0.3885 ns | 113.583 ns | 0.0074 |      12 B |
    /// |       WithReflection | 369.719 ns | 6.9477 ns | 5.4243 ns | 367.856 ns | 0.0229 |      36 B |
    /// | WithReflectionCached | 281.290 ns | 0.9358 ns | 0.7814 ns | 281.415 ns | 0.0148 |      24 B |
    /// |           WithLambla |  17.391 ns | 0.0700 ns | 0.0621 ns |  17.373 ns | 0.0076 |      12 B |
    /// |  WithCustomActivator |  27.644 ns | 0.6665 ns | 1.3155 ns |  27.039 ns | 0.0076 |      12 B |
    /// </summary>
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
        /// A constructor with 0 arguments
        /// </summary>
        public Func<object> Ctr0 { get;  }

        /// <summary>
        /// A constructor with 1 arugument
        /// </summary>
        public Func<object, object> Ctr1 { get; }

        /// <summary>
        /// A constructor with 2 arguments
        /// </summary>
        public Func<object, object, object> Ctr2 { get; }

        /// <summary>
        /// A constructor with 3 arguments
        /// </summary>
        public Func<object, object, object, object> Ctr3 { get; }

        /// <summary>
        /// A constructor with 4 arguments
        /// </summary>
        public Func<object, object, object, object, object> Ctr4 { get; }

        /// <summary>
        /// A constructor with 5 arguments
        /// </summary>
        public Func<object, object, object, object, object, object> Ctr5 { get; }

        /// <summary>
        /// Construct a Jedi Ctr Info with a given Reflection Constructor Info
        /// </summary>
        /// <param name="info"></param>
        public JediCtrInfo(ConstructorInfo info)
        {
            Info = info;
            Parameters = Info.GetParameters().Select(p => p.ParameterType).ToArray();

            // Compile the constructor if needed
            if (Registry.InjectMechanism == InjectMechanism.Compiled)
            {
                switch (Parameters.Length)
                {
                    case 0:
                        Ctr0 = Expression.Lambda<Func<object>>(
                            Expression.New(Info, Array.Empty<ParameterExpression>())).Compile();
                        break;
                    case 1:
                        Ctr1 = Expression.Lambda<Func<object, object>>(
                            Expression.New(Info, new[] { 
                                Expression.Parameter(typeof(object)) 
                            })).Compile();
                        break;
                    case 2:
                        Ctr2 = Expression.Lambda<Func<object, object, object>>(
                            Expression.New(Info, new[] {
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                            })).Compile();
                        break;
                    case 3:
                        Ctr3 = Expression.Lambda<Func<object, object, object, object>>(
                            Expression.New(Info, new[] {
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                            })).Compile();
                        break;
                    case 4:
                        Ctr4 = Expression.Lambda<Func<object, object, object, object, object>>(
                            Expression.New(Info, new[] {
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                            })).Compile();
                        break;
                    case 5:
                        Ctr5 = Expression.Lambda<Func<object, object, object, object, object, object>>(
                            Expression.New(Info, new[] {
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                                Expression.Parameter(typeof(object)),
                            })).Compile();
                        break;
                    default:
                        throw new Exception("Only a  maximum of 5 parameters are currently supported!");
                }
            }
        }

        /// <summary>
        /// Spawn an instance with the given parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object Spawn(object[] parameters)
        {
            switch (Registry.InjectMechanism)
            {
                case InjectMechanism.Compiled:
                    switch (Parameters.Length)
                    {
                        case 0:
                            return Ctr0();
                        case 1:
                            return Ctr1(parameters[0]);
                        case 2:
                            return Ctr2(parameters[0], parameters[1]);
                        case 3:
                            return Ctr3(parameters[0], parameters[1], parameters[2]);
                        case 4:
                            return Ctr4(parameters[0], parameters[1], parameters[2], parameters[3]);
                        case 5:
                            return Ctr5(parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                        default:
                            throw new Exception("Only a maximum of 5 parameters are currently supported!");
                    }
                case InjectMechanism.Reflection:
                    return Info.Invoke(parameters);
                default:
                    throw new Exception("The constructor was not found!");
            }
        }
    }
}
