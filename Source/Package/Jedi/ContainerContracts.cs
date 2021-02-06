using System;
using System.Collections.Generic;
using System.Linq;

namespace Jedi
{
    public partial class DiContainer
    {
        #region Contracts

        /// <summary>
        /// Tries to find the contract in the container and the registered parents
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private Contract TryGetContractFromContainerAndParents(object id)
        {
            // Create the empty contract and contract id
            Contract contract = null;
            
            // Check if the contract exists in the container
            if (_contracts.ContainsKey(id))
            {
                contract = _contracts[id];
            }
            else
            {
                // Check if there are any parents
                if (_parents != null)
                {
                    // Try to find the contract in the parent
                    foreach (DiContainer parent in _parents)
                    {
                        // Try to get the contract fromthe parent
                        contract = parent.TryGetContract(id);

                        // The contract was found, we can stop looping and return that contract
                        if (contract != null)
                        {
                            break;
                        }
                    }
                }
            }
            
            return contract;
        }

        /// <summary>
        /// Try to obtain a contract
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract TryGetContract(object id)
        {
            return TryGetContractFromContainerAndParents(id);
        }

        /// <summary>
        /// Obtain a contract for a given type and id
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract GetContract(object id)
        {
            Contract contract = TryGetContract(id);

            // Check if the contract is not null
            if (contract == null)
            {
                throw new Exception($"Could not find a contract with the given id : {id}!");
            }
            
            return contract;
        }

        /// <summary>
        /// Tries to find the contract in the container and the registered parents
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<Contract> GetAllContractsFromContainerAndParents(object id)
        {
            // Check if the contract exists in the container
            if (_contracts.ContainsKey(id))
            {
                yield return _contracts[id];
            }

            // Check if the provided type is an interface
            if (id is Type {IsInterface: true} type)
            {
                // Traverse the contracts that implement this interface
                foreach (Contract contract in _contracts.Values)
                {
                    if (contract.Interfaces.Contains(type))
                    {
                        yield return contract;
                    }
                }
            }

            // Check if there are any parents
            if (_parents != null)
            {
                // Try to find the contract in the parent
                foreach (DiContainer parent in _parents)
                {
                    // Try to get the contract fromthe parent
                    foreach (Contract contract in parent.GetAllContractsFromContainerAndParents(id))
                    {
                        yield return contract;
                    }
                }
            }
        }

        /// <summary>
        /// Gat all matching contracts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Contract> GetAllContracts(object id)
        {
            return GetAllContractsFromContainerAndParents(id);
        }

        /// <summary>
        /// Get all contracts of a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Contract> GetAllContracts<T>()
        {
            return GetAllContracts(typeof(T));
        }

        #endregion
    }
}
