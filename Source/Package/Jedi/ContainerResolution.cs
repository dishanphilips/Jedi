using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial  class DiContainer
    {
        #region Resolve

        public object Resolve(Type type, object id = null)
        {
            return null;
        }

        public T Resolve<T>(object id = null)
        {
            return (T)Resolve(typeof(T), id);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return null;
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return null;
        }

        public async Task<object> ResolveAsync(Type type, object id = null)
        {
            return null;
        }

        public async Task<T> ResolveAsync<T>(Type type, object id = null)
        {
            return (T)await ResolveAsync(typeof(T), id);
        }

        public async IAsyncEnumerable<object> ResolveAllAsync(Type type)
        {
            yield return null;
        }

        public async IAsyncEnumerable<T> ResolveAllAsync<T>()
        {
            yield return default(T);
        }

        #endregion
    }
}
