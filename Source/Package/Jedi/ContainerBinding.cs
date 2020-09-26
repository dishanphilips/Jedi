using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    public partial class DiContainer
    {
        #region Binding

        /// <summary>
        /// Bind a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract Bind(Type type, object id = null)
        {
            // Check if a contract already exists
            if (_contracts.ContainsKey(Contract.GetContractId(type, id)))
            {
                throw new Exception("A contract exists with the given Type and Id!");
            }
            else
            {
                // Create the contract
                Contract contract = new Contract(this, type, id);

                // Add the contract
                _contracts.TryAdd(contract.Id, contract);

                // Return the added contract
                return contract;
            }
        }

        /// <summary>
        /// Bind a given type of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract Bind<T>(object id = null)
        {
            return Bind(typeof(T), id);
        }

        /// <summary>
        /// Bind a given type and id
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract BindId(Type type, object id)
        {
            return Bind(type, id);
        }

        /// <summary>
        /// Bind a given T and instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract BindId<T>(object id)
        {
            return BindId(typeof(T), id);
        }

        /// <summary>
        /// Bind a given instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract BindInstance(object instance, object id  = null)        
        {
            return Bind(instance.GetType(), id).WithInstance(instance);
        }

        #endregion
    }
}
