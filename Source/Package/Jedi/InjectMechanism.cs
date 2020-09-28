using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    public enum InjectMechanism
    {
        /// <summary>
        /// Inject using regular reflection
        /// </summary>
        Reflection,

        /// <summary>
        /// Inject using Compiled Delegates
        /// </summary>
        Compiled
    }
}
