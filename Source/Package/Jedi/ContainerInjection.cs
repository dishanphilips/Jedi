using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial class DiContainer
    {
        #region Inection

        /// <summary>
        /// Inject a given instance
        /// </summary>
        /// <param name="injectable"></param>
        public void Inject(object injectable)
        {
            // Ensure that the injectable is not null
            if (injectable != null)
            {
                // Obtain the type info
                TypeInfo typeInfo = Registry.GetTypeInfo(injectable.GetType());

                // Inject fields
                foreach (JediFieldInfo field in typeInfo.Fields)
                {
                    field.Set(injectable, Resolve(field.Info.FieldType, field.Id));
                }

                // Inject properties
                foreach (JediPropertyInfo property in typeInfo.Properties)
                {
                    property.Set(injectable, Resolve(property.Info.PropertyType, property.Id));
                }

                // Inject methods
                foreach (JediMethodInfo method in typeInfo.Methods)
                {
                    // Resolve all parameters
                    object[] parameters = new object[method.Parameters.Length];
                    for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                    {
                        // Resolve the value from the container
                        parameters[parameterIndex] = Resolve(method.Parameters[parameterIndex]);
                    }

                    // Invoke the method
                    method.Invoke(injectable, parameters);
                }
            }
        }

        /// <summary>
        /// Inject a given set of instances
        /// </summary>
        /// <param name="injectables"></param>
        public void Inject(IEnumerable<object> injectables)
        {
            // Obtain the type info
            TypeInfo typeInfo = Registry.GetTypeInfo(injectables.First().GetType());

            // Inject fields
            foreach (var injectable in injectables)
            {
                // Ensure that the injectable is not null
                if (injectable != null)
                {
                    foreach (JediFieldInfo field in typeInfo.Fields)
                    {
                        field.Set(injectable, Resolve(field.Info.FieldType, field.Id));
                    }
                }
            }            

            // Inject properties
            foreach (var injectable in injectables)
            {
                // Ensure that the injectable is not null
                if (injectable != null)
                {
                    foreach (JediPropertyInfo property in typeInfo.Properties)
                    {
                        property.Set(injectable, Resolve(property.Info.PropertyType, property.Id));
                    }
                }
            }           

            // Inject methods
            foreach (var injectable in injectables)
            {
                // Ensure that the injectable is not null
                if (injectable != null)
                {
                    foreach (JediMethodInfo method in typeInfo.Methods)
                    {
                        // Resolve all parameters
                        object[] parameters = new object[method.Parameters.Length];
                        for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                        {
                            // Resolve the value from the container
                            parameters[parameterIndex] = Resolve(method.Parameters[parameterIndex]);
                        }

                        // Invoke the method
                        method.Invoke(injectable, parameters);
                    }
                }
            }            
        }

        /// <summary>
        /// Inject an instance asynchronously
        /// </summary>
        /// <param name="injectable"></param>
        /// <returns></returns>
        public async Task InjectAsync(object injectable)
        {
            // Obtain the type info
            TypeInfo typeInfo = Registry.GetTypeInfo(injectable.GetType());

            // Inject everything in parrallel
            List<Task> injectTasks = new List<Task>();

            // Inject fields
            foreach (JediFieldInfo field in typeInfo.Fields)
            {
                injectTasks.Add(Task.Run(async () => {
                    field.Set(injectable, await ResolveAsync(field.Info.FieldType, field.Id));
                }));               
            }

            // Inject properties
            foreach (JediPropertyInfo property in typeInfo.Properties)
            {
                injectTasks.Add(Task.Run(async () => {
                    property.Set(injectable, await ResolveAsync(property.Info.PropertyType, property.Id));
                }));
            }

            // Inject methods
            foreach (JediMethodInfo method in typeInfo.Methods)
            {
                injectTasks.Add(Task.Run(async () => {
                    // Resolve all parameters
                    object[] parameters = new object[method.Parameters.Length];
                    for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                    {
                        // Resolve the value from the container
                        parameters[parameterIndex] = await ResolveAsync(method.Parameters[parameterIndex]);
                    }

                    // Invoke the method
                    method.Invoke(injectable, parameters);
                }));
            }

            // Wait untill all inject tasks are compelted
            await Task.WhenAll(injectTasks);
        }

        /// <summary>
        /// Inject a set of instances asynchronously
        /// </summary>
        /// <param name="injectables"></param>
        /// <returns></returns>
        public async Task InjctAsync(IEnumerable<object> injectables)
        {
            // Obtain the type info
            TypeInfo typeInfo = Registry.GetTypeInfo(injectables.First().GetType());

            // Inject everything in parrallel
            List<Task> injectTasks = new List<Task>();

            foreach (object injectable in injectables)
            {
                // Inject fields
                foreach (JediFieldInfo field in typeInfo.Fields)
                {
                    injectTasks.Add(Task.Run(async () => {
                        field.Set(injectable, await ResolveAsync(field.Info.FieldType, field.Id));
                    }));
                }

                // Inject properties
                foreach (JediPropertyInfo property in typeInfo.Properties)
                {
                    injectTasks.Add(Task.Run(async () => {
                        property.Set(injectable, await ResolveAsync(property.Info.PropertyType, property.Id));
                    }));
                }

                // Inject methods
                foreach (JediMethodInfo method in typeInfo.Methods)
                {
                    injectTasks.Add(Task.Run(async () => {
                        // Resolve all parameters
                        object[] parameters = new object[method.Parameters.Length];
                        for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                        {
                            // Resolve the value from the container
                            parameters[parameterIndex] = await ResolveAsync(method.Parameters[parameterIndex]);
                        }

                        // Invoke the method
                        method.Invoke(injectable, parameters);
                    }));
                }
            }

            // Wait untill all inject tasks are compelted
            await Task.WhenAll(injectTasks);
        }

        #endregion
    }
}
