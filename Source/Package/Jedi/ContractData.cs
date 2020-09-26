using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial class Contract
    {
        #region ContractData

        /// <summary>
        /// Internal field to store a provided id
        /// This is used when the id needs to be customized
        /// </summary>
        private object _id;

        /// <summary>
        /// Provided instance
        /// </summary>
        private bool _instanceProvided;

        /// <summary>
        /// The container this contract belongs to
        /// </summary>
        private readonly DiContainer _container;

        /// <summary>
        /// A helper property to get the id
        /// </summary>
        public object Id { get { return GetContractId(Type, _id); } }

        /// <summary>
        /// The true type of the contract
        /// </summary>
        public Type Type { get; private set;}

        /// <summary>
        /// The instance of the contract
        /// This will be used onnly on Single instances
        /// </summary>
        public object Instance { get; private set; }

        /// <summary>
        /// The type of instance
        /// This will dictate what factory method to use when a contract is being resolved
        /// </summary>
        public InstanceType InstanceType { get; private set; } = InstanceType.SingleOnResolve;

        /// <summary>
        /// A alias type to resolve the true type
        /// </summary>
        public Type ToType { get; private set; }

        /// <summary>
        /// Specifies the contract to resolve this contract from
        /// This can be used to chain contracts
        /// </summary>
        public Contract From { get; private set; }

        /// <summary>
        /// Specifies interfaces associated with this type
        /// </summary>
        public Type[] Interfaces { get; private set; }

        /// <summary>
        /// If specified, this method will be used to instantiate the instance
        /// </summary>
        public Func<object> Method { get; private set; }

        /// <summary>
        /// If specified this method will be used to instantiate the instance asynchronously
        /// </summary>
        public Task<Func<object>> MethodAsync { get; private set; }

        /// <summary>
        /// Delegate for insantiated
        /// </summary>
        public delegate void Insantiated();

        /// <summary>
        /// Fired when Jedi instantiates the instance
        /// Only fired when the object is constructed using Jedi and NOT other methods
        /// </summary>
        public Insantiated Instantiated { get; set; }

        /// <summary>
        /// Delegate for async instantiated
        /// </summary>
        public delegate void InsantiatedAsync();

        /// <summary>
        /// Fired when Jedi instantiates the instance asynchronously
        /// Only fired when the object is constructed using Jedi and NOT other methods
        /// </summary>
        public InsantiatedAsync InstantiatedAsync { get; private set; }

        /// <summary>
        /// Only the Type needs to be set, the remaining can be set be using the builder
        /// If an Id is provided, it will be how the contract wil be referenced
        /// </summary>
        /// <param name="container"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public Contract(DiContainer container, Type type, object id = null)
        {
            _container = container;
            Type = type;
            _id = id;
        }

        #endregion

        #region ContractHelper

        /// <summary>
        /// Get the contract Id for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static object GetContractId(Type type, object id)
        {
            if (id != null)
            {
                return id;
            }
            else
            {
                return type;
            }
        }

        #endregion
    }
}
