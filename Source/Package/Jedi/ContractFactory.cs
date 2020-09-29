using System.Threading.Tasks;

namespace Jedi
{
    public partial class Contract
    {
        #region SynchronousInstantiation

        /// <summary>
        /// Get the instance of the contract
        /// </summary>
        /// <returns></returns>
        public object GetInstance()
        {
            // Set the isntance type to Single if it was never resolved before
            if (InstanceType == InstanceType.SingleOnResolve)
            {
                InstanceType = InstanceType.Single;
            }

            // Create a placeholder instance
            object instance = Instance;

            // Flag to determine if instaiated by Jedi
            bool jediInstantiated = false;

            try 
            {
                // Lock if it is a singleton
                if (InstanceType == InstanceType.Single)
                {
                    _instantiationLock.Wait();
                }

                // Run the factory method only if the instance is null
                // and the instance was not provided
                if (instance == null && _instanceProvided == false)
                {
                    // Check if the contract must be resolved from a chain contractr
                    if (From != null)
                    {
                        instance = From.GetInstance();
                    }
                    // Check if the contract must be resolved from a method
                    else if (Method != null)
                    {
                        instance = Method();
                    }
                    // Instantiate
                    else
                    {
                        instance = Instantiate();

                        // Set the Jedi Instantiated flag
                        jediInstantiated = true;
                    }
                }
            }
            finally
            {
                if (InstanceType == InstanceType.Single)
                {
                    // Set the instance
                    Instance = instance;

                    // Release Lock if it is a singleton
                    _instantiationLock.Release();
                }
            }

            // If the instance was Jedi instantiated
            if (jediInstantiated)
            {
                // Inject the instance
                _container.Inject(instance);

                // Invoke the on instantiated method
                Instantiated?.Invoke();
            }

            return instance;
        }

        /// <summary>
        /// Instantiate a contract
        /// </summary>
        /// <returns></returns>
        private object Instantiate()
        {
            // Get the type info
            TypeInfo typeInfo = Registry.GetTypeInfo(Type);

            // Create a new parameter array for the constructor
            object[] parameters = new object[typeInfo.Ctr.Parameters.Length];

            // Resolve all the required parameters
            for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                parameters[parameterIndex] = _container.Resolve(typeInfo.Ctr.Parameters[parameterIndex]);
            }

            // Spawn the instance
            object instance = typeInfo.Ctr.Spawn(parameters);

            return instance;
        }

        #endregion

        #region AsynchronousInstantiation

        /// <summary>
        /// Get an instance of the contract asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetInstanceAsync()
        {
            // Set the isntance type to single if it was never resolved before
            if (InstanceType == InstanceType.SingleOnResolve)
            {
                InstanceType = InstanceType.Single;
            }

            // Create a placeholder instance
            object instance = Instance;

            // Flag to determine if instaiated by Jedi
            bool jediInstantiated = false;

            try
            {
                // Lock if it is a singleton
                if (InstanceType == InstanceType.Single)
                {
                    await _instantiationLock.WaitAsync();
                }

                // Only run the factory if the instance is null
                // And the isntance was not proivded
                if (instance == null && _instanceProvided == false)
                {
                    // Check if the contract must be resolved from a chain contractr
                    if (From != null)
                    {
                        instance = From.GetInstanceAsync();
                    }
                    // Check if the contract must be resolved from a method
                    else if (MethodAsync != null)
                    {
                        instance = await MethodAsync();
                    }
                    // Check if the contract must be resolved from a method
                    else if (Method != null)
                    {
                        instance = Method();
                    }
                    // Instantiate
                    else
                    {
                        instance = await InstantiateAsync();

                        // Set the Jedi Instantiated flag
                        jediInstantiated = true;
                    }
                }
            }
            finally 
            {
                
                if (InstanceType == InstanceType.Single)
                {
                    // Set the instance
                    Instance = instance;

                    // Release Lock if it is a singleton
                    _instantiationLock.Release();
                }
            }

            // If the instance was Jedi instantiated
            if (jediInstantiated)
            {
                // Inject the instance
                await _container.InjectAsync(instance);

                // Invoke the InstantiatedAsync metod
                await InstantiatedAsync?.Invoke();

                // Invoke the Instantiated method
                Instantiated?.Invoke();
            }

            return instance;
        }

        /// <summary>
        /// Instantiate a contract asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task<object> InstantiateAsync()
        {
            // Get the type info
            TypeInfo typeInfo = Registry.GetTypeInfo(Type);

            // Create a new parameter array for the constructor
            object[] parameters = new object[typeInfo.Ctr.Parameters.Length];

            // Resolve all the required parameters asynchronsouly
            for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
            {
                parameters[parameterIndex] = await _container.ResolveAsync(typeInfo.Ctr.Parameters[parameterIndex]);
            }

            // Spawn the instance
            object instance = typeInfo.Ctr.Spawn(parameters);

            return instance;
        }

        #endregion
    }
}
