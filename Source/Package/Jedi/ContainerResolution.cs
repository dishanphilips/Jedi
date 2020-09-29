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
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object Resolve(Type type, object id = null)
        {
            return GetContract(type, id).GetInstance();
        }

        /// <summary>
        /// Resolve a given instance in a container and parent containers
        /// asynchronously for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> ResolveAsync(Type type, object id = null)
        {
            return await GetContract(type, id).GetInstanceAsync();
        }

        /// <summary>
        /// Resolve  an instance from the container and parent containers
        /// for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Resolve<T>(object id = null)
        {
            return (T)Resolve(typeof(T), id);
        }

        /// <summary>
        /// Resolve a given instance in a container and parent containers
        /// asynchronously for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> ResolveAsync<T>(Type type, object id = null)
        {
            return (T)await ResolveAsync(typeof(T), id);
        }

        /// <summary>
        /// Try to obtain an instance from the container and the parent containers
        /// if there is a contract for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public object TryResolve(Type type, object id = null)
        {
            return TryGetContract(type, id)?.GetInstance();
        }

        /// <summary>
        /// Try to obtain an instance asynchronously from the container and the parent containers
        /// if there is a contract for a given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> TryResolveAsync(Type type, object id = null)
        {
            return await TryGetContract(type, id)?.GetInstanceAsync();
        }

        /// <summary>
        /// Try to obtain an instance from the container and the parent containers
        /// if there is a contract for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public object TryResolve<T>(object id = null)
        {
            return TryResolve(typeof(T), id);
        }

        /// <summary>
        /// Try to obtain an instance asynchronously from the container and the parent containers
        /// if there is a contract for a given T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> TryResolveAsync<T>(object id = null)
        {
            return await TryResolveAsync(typeof(T), id);
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
