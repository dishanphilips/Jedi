using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    public enum InstanceType
    {
        /// <summary>
        /// Will be considered a Singleton only when resolved for the firs time
        /// </summary>
        SingleOnResolve,

        /// <summary>
        /// A Singleton instance
        /// </summary>
        Single,

        /// <summary>
        /// Instantited On Each resolve
        /// </summary>
        Transient
    }
}
