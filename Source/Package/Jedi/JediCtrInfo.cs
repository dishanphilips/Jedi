using System;
using System.Collections.Generic;
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
        public Func<object[], object> Ctr { get;  }

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
                // Create the parameters required
                ParameterExpression paramExpr = Expression.Parameter(typeof(object[]), "arguments");

                // Create all the arguments and the conversions required
                List<Expression> argExprs = new List<Expression>();
                for (int parameterIndex = 0; parameterIndex < Parameters.Length; parameterIndex++)
                {
                    BinaryExpression indexedAcccess = Expression.ArrayIndex(paramExpr, Expression.Constant(parameterIndex));
                    if (Parameters[parameterIndex].IsClass == false && Parameters[parameterIndex].IsInterface == false)
                    {
                        ParameterExpression localVariable = Expression.Variable(Parameters[parameterIndex], "localVariable");

                        BlockExpression block = Expression.Block(new[] { localVariable },
                                Expression.IfThenElse(Expression.Equal(indexedAcccess, Expression.Constant(null)),
                                    Expression.Assign(localVariable, Expression.Default(Parameters[parameterIndex])),
                                    Expression.Assign(localVariable, Expression.Convert(indexedAcccess, Parameters[parameterIndex]))
                                ),
                                localVariable
                            );

                        argExprs.Add(block);

                    }
                    else
                    {
                        argExprs.Add(Expression.Convert(indexedAcccess, Parameters[parameterIndex]));
                    }
                }

                // Throw an excpetion if the number of parameters is incorrect
                ConditionalExpression argLenExpr = 
                    Expression.IfThen(
                        Expression.NotEqual(Expression.Property(paramExpr, typeof(object[]).GetProperty("Length")), 
                        Expression.Constant(Parameters.Length)),
                        Expression.Throw(Expression.New(typeof(ArgumentException).GetConstructor(new Type[] { typeof(string) }), 
                        Expression.Constant("The length does not match parameters number")))
                    );

                // Create the new statement
                NewExpression newExpr = Expression.New(Info, argExprs);

                // Create the final constructor
                BlockExpression ctrExprWithArgs = Expression.Block(argLenExpr, Expression.Convert(newExpr, typeof(object)));

                // Compile the constructor
                Ctr = Expression.Lambda(ctrExprWithArgs, new[] { paramExpr }).Compile() as Func<object[], object>;
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
                    return Ctr(parameters);
                case InjectMechanism.Reflection:
                    return Info.Invoke(parameters);
                default:
                    throw new Exception("The constructor was not found!");
            }
        }
    }
}
