using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jedi
{
    public class TypeInfo
    {
        /// <summary>
        /// A cached copy of binding flags
        /// </summary>
        private static BindingFlags BindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// The underlying type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// If the type is disposable
        /// </summary>
        bool IsDisposable { get; }

        /// <summary>
        /// Details about the constructor of the type
        /// </summary>
        public JediCtrInfo Ctr { get; }

        /// <summary>
        /// All fields that required injection
        /// </summary>
        public List<JediFieldInfo> Fields { get; } = new List<JediFieldInfo>();

        /// <summary>
        /// All properties that rquired injection
        /// </summary>
        public List<JediPropertyInfo> Properties { get; } = new List<JediPropertyInfo>();

        /// <summary>
        /// All methods that require injection
        /// </summary>
        public List<JediMethodInfo> Methods { get; } = new List<JediMethodInfo>();

        /// <summary>
        /// Initilaize a JediType
        /// </summary>
        /// <param name="type"></param>
        public TypeInfo(Type type)
        {
            // Set the type
            this.Type = type;

            // Set if disposable
            IsDisposable = this.Type.GetInterfaces().Contains(typeof(IDisposable));

            // Set the constructor
            ConstructorInfo[] ctrs = this.Type.GetConstructors();
            Ctr = new JediCtrInfo( ctrs.FirstOrDefault(c => c.GetCustomAttribute<InjectAttribute>() != null) ?? 
                                    ctrs.First());

            // Loop through the type and all the base types
            Type currentType = Type;
            while (currentType != null)
            {
                // Set the fields
                foreach (FieldInfo field in currentType.GetFields(BindingFlag))
                {
                    if (field.GetCustomAttribute<InjectAttribute>() != null)
                    {
                        Fields.Add(new JediFieldInfo(field));
                    }
                }

                // Set the properties
                foreach (PropertyInfo property in currentType.GetProperties(BindingFlag))
                {
                    if ( property.CanWrite && property.GetCustomAttribute<InjectAttribute>() != null)
                    {
                        Properties.Add(new JediPropertyInfo(property));
                    }
                }

                // Set the methods
                foreach (MethodInfo method in currentType.GetMethods(BindingFlag))
                {
                    if (method.GetCustomAttribute<InjectAttribute>() != null)
                    {
                        Methods.Add(new JediMethodInfo(method));
                    }
                }

                // Move on to the base type
                currentType = currentType.BaseType;
            }
        }
    }
}
