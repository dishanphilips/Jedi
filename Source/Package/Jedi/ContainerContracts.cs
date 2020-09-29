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
        private Contract TryGetContractFromContainerAndParents(Type type, object id = null)
        {
            // Create the empty contract and contract id
            Contract contract = null;
            object contractId = Contract.GetContractId(type, id);

            // Check if the contract exists in the container
            if (_contracts.ContainsKey(contractId))
            {
                contract = _contracts[contractId];
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
                        contract = parent.TryGetContract(type, id);

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
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract TryGetContract(Type type, object id = null)
        {
            return TryGetContractFromContainerAndParents(type, id);
        }

        /// <summary>
        /// Obtain a contract for a given type and id
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Contract GetContract(Type type, object id = null)
        {
            Contract contract = TryGetContract(type, id);

            // Check if the contract is not null
            if (contract == null)
            {
                return contract;
            }
            else
            {
                throw new Exception($"Could not find a contract with the given type : {type.AssemblyQualifiedName}!");
            }
        }

        /// <summary>
        /// Tries to find the contract in the container and the registered parents
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private IEnumerable<Contract> GetAllContractsFromContainerAndParents(Type type, object id = null)
        {
            // Create contract id
            object contractId = Contract.GetContractId(type, id);

            // Check if the contract exists in the container
            if (_contracts.ContainsKey(contractId))
            {
                yield return _contracts[contractId];
            }

            // Check if the provided type is an interface
            if (type.IsInterface)
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
                    foreach (Contract contract in parent.GetAllContractsFromContainerAndParents(type, id))
                    {
                        yield return contract;
                    }
                }
            }
        }

        /// <summary>
        /// Gat all matching contracts
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Contract> GetAllContracts(Type type, object id = null)
        {
            return GetAllContractsFromContainerAndParents(type, id);
        }

        /// <summary>
        /// Get all contracts of a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Contract> GetAllContracts<T>(object id = null)
        {
            return GetAllContracts(typeof(T), id);
        }

        #endregion
    }
}
