using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial  class DiContainer
    {
        #region Resolve

        /// <summary>
        /// Resolve an instance from the container and parent containers
        /// for a given type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Resolve(object id)
        {
            return GetContract(id).GetInstance();
        }

        /// <summary>
        /// Resolve a given instance in a container and parent containers
        /// asynchronously for a given type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> ResolveAsync(object id = null)
        {
            return await GetContract(id).GetInstanceAsync();
        }

        /// <summary>
        /// Resolve  an instance from the container and parent containers
        /// for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve a given instance in a container and parent containers
        /// asynchronously for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> ResolveAsync<T>()
        {
            return (T)await ResolveAsync(typeof(T));
        }

        /// <summary>
        /// Try to obtain an instance from the container and the parent containers
        /// if there is a contract for a given type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object TryResolve(object id = null)
        {
            return TryGetContract(id)?.GetInstance();
        }

        /// <summary>
        /// Try to obtain an instance asynchronously from the container and the parent containers
        /// if there is a contract for a given type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> TryResolveAsync(object id)
        {
            return await TryGetContract(id).GetInstanceAsync();
        }

        /// <summary>
        /// Try to obtain an instance from the container and the parent containers
        /// if there is a contract for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T TryResolve<T>()
        {
            return (T)TryResolve(typeof(T));
        }

        /// <summary>
        /// Try to obtain an instance asynchronously from the container and the parent containers
        /// if there is a contract for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> TryResolveAsync<T>()
        {
            return (T)await TryResolveAsync(typeof(T));
        }

        /// <summary>
        /// Resolve all instances from the container and parent containers
        /// for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            foreach (Contract contract in GetAllContracts(type))
            {
                yield return contract.GetInstance();
            }
        }

        /// <summary>
        /// Resolve all instances in a container and parent containers
        /// aynchronosly for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async IAsyncEnumerable<object> ResolveAllAsync(Type type)
        {
            foreach (Contract contract in GetAllContracts(type))
            {
                yield return await contract.GetInstanceAsync();
            }
        }

        /// <summary>
        /// Resolve all instances from a container and parent containers
        /// for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)ResolveAll(typeof(T));
        }

        /// <summary>
        /// Resolve all instances in a container and parent containers
        /// aynchronosly for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async IAsyncEnumerable<T> ResolveAllAsync<T>()
        {
            foreach (Contract contract in GetAllContracts(typeof(T)))
            {
                yield return (T)(await contract.GetInstanceAsync());
            }
        }

        #endregion
    }
}
