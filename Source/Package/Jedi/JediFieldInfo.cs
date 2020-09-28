using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediFieldInfo
    {
        /// <summary>
        /// The Field Information
        /// </summary>
        public FieldInfo Info { get; }

        /// <summary>
        /// Injection Id
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// Injection optional flag
        /// </summary>
        public object Optional { get; }

        /// <summary>
        /// Compiled Set Action
        /// </summary>
        public Action<object, object> SetAction { get; }

        /// <summary>
        /// Construct Jedi Field Info with a given FieldInfo
        /// </summary>
        /// <param name="info"></param>
        public JediFieldInfo(FieldInfo info)
        {
            // Get the attribute
            InjectAttribute injectAttribute = info.GetCustomAttribute<InjectAttribute>();

            // Set the data
            Info = info;
            Id = injectAttribute.Id;
            Optional = injectAttribute.Optional;

            // Compile the delegate if required
            if (Registry.InjectMechanism == InjectMechanism.Compiled)
            {
                // Creat the expression parameters
                ParameterExpression targetParameter = Expression.Parameter(typeof(object));
                ParameterExpression valueParameter = Expression.Parameter(typeof(object));

                // Create the property expression
                MemberExpression fieldExpression = Expression.Field(Expression.Convert(
                                                        targetParameter, Info.FieldType), Info);
                // Create the lambda
                SetAction = Expression.Lambda<Action<object, object>>(
                    Expression.Assign(fieldExpression, Expression.Convert(valueParameter, Info.FieldType)),
                    targetParameter,
                    valueParameter).Compile();
            }
        }

        /// <summary>
        /// Set the field in a given target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void Set(object target, object value)
        {
            switch (Registry.InjectMechanism)
            {
                case InjectMechanism.Compiled:
                    SetAction(target, value);
                    break;
                case InjectMechanism.Reflection:
                    Info.SetValue(target, value);
                    break;
            }
        }
    }
}
