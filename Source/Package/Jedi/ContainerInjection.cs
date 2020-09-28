using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Jedi
{
    public partial class DiContainer
    {
        #region Inection

        public void Inject(object injectable)
        { 
        }

        public void Inject(IEnumerable<object> injectables)
        { 
        }

        public async Task InjectAsync(object injectable)
        { 
        }

        public async Task InjctAsync(IEnumerable<object> injectables)
        { 
        }

        #endregion
    }
}
