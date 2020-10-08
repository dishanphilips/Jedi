using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Jedi
{
    public class JediPropertyInfo
    {
        /// <summary>
        /// The property information
        /// </summary>
        public PropertyInfo Info { get; }

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
        /// Construct Jedi Property Info witha given PropertyInfo
        /// </summary>
        /// <param name="info"></param>
        public JediPropertyInfo(PropertyInfo info)
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
                MemberExpression propertyExpression = Expression.Property(
                    Expression.Convert(targetParameter, Info.DeclaringType), 
                    Info
                );

                // Create the lambda
                SetAction = Expression.Lambda<Action<object, object>>(
                    Expression.Assign(propertyExpression, Expression.Convert(valueParameter, Info.PropertyType)),
                    targetParameter,
                    valueParameter
                ).Compile();
            }
        }

        /// <summary>
        /// Set the property in a given target
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
