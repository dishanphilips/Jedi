using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial class Contract
    {
        #region ContractBuilder

        /// <summary>
        /// Include an Id to a contract
        /// This Id will be used instead of the Type when being resolved
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract WithId(object id)
        {
            _id = id;
            return this;
        }

        /// <summary>
        /// Include an Instance for a contract
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public Contract WithInstance(object instance)
        {
            Instance = instance;
            _instanceProvided = true;
            return this;
        }

        /// <summary>
        /// Specify that the Instance of the contract should be a Singleton
        /// </summary>
        /// <returns></returns>
        public Contract AsSingle()
        {
            InstanceType = InstanceType.Single;
            return this;
        }

        /// <summary>
        /// Specifies that the Instance of the contract is Transient
        /// This means that a new instance will be Spawned, each time it is resolved
        /// </summary>
        /// <returns></returns>
        public Contract AsTransient()
        {
            InstanceType = InstanceType.Transient;
            return this;
        }

        /// <summary>
        /// Resolve the contract from the specified Type
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public Contract To(Type to)
        {
            ToType = to;
            return this;
        }

        /// <summary>
        /// A generic implementation of the ToType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Contract To<T>()
        {
            return To(typeof(T));
        }

        /// <summary>
        /// Chain the contract to another contract
        /// </summary>
        /// <param name="fromContract"></param>
        /// <returns></returns>
        public Contract ResolveFrom(Contract fromContract)
        {
            From = fromContract;
            return this;
        }

        /// <summary>
        /// Chain the contract to a different contract
        /// The contract will be resolved from the Container
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract ResolveFrom(Type fromType, object id = null)
        {
            return ResolveFrom(_container.GetContract(fromType, id));
        }

        /// <summary>
        /// A generic implementation of the ResolveFrom method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract ResolveFrom<T>(object id = null)
        {
            return ResolveFrom(typeof(T), id);
        }

        /// <summary>
        /// Include a method to instantiate an instance from
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public Contract FromMethod(Func<object> method)
        {
            Method = method;
            return this;
        }

        /// <summary>
        /// Include a method to instantiate from asynchronously
        /// </summary>
        /// <param name="methodAsync"></param>
        /// <returns></returns>
        public Contract FromMethodAsync(Task<Func<object>> methodAsync)
        {
            MethodAsync = methodAsync;
            return this;
        }

        /// <summary>
        /// Include a call back to be called when a contract is instantiated
        /// </summary>
        /// <param name="onInstantiated"></param>
        /// <returns></returns>
        public Contract OnInstantiated(Insantiated onInstantiated)
        {
            Instantiated += onInstantiated;
            return this;
        }

        /// <summary>
        /// Include a callback to be called when a contract is instantiated asynchronously
        /// </summary>
        /// <param name="onInsantiatedAsync"></param>
        /// <returns></returns>
        public Contract OnInstantiatedAsync(InsantiatedAsync onInsantiatedAsync)
        {
            InstantiatedAsync += onInsantiatedAsync;
            return this;
        }

        #endregion
    }
}
