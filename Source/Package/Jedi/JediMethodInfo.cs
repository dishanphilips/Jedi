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
        public Action<object, object[]> Method { get; }

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
                MethodCallExpression callExpr = Expression.Call(Info, argExprs);

                // Create the final constructor
                BlockExpression callExprWithArgs = Expression.Block(argLenExpr, Expression.Convert(callExpr, typeof(object)));

                // Compile the constructor
                Method = Expression.Lambda(callExprWithArgs, new[] { paramExpr }).Compile() as Action<object, object[]>;
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
                    Method.Invoke(target, parameters);
                    break;
                case InjectMechanism.Reflection:
                    Info.Invoke(target, parameters);
                    break;
            }            
        }
    }
}
