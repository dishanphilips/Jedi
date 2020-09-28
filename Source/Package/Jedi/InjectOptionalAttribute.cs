using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectOptionalAttribute : InjectAttribute
    {
        /// <summary>
        /// A shorthand attribute for optional injection
        /// </summary>
        /// <param name="id"></param>
        public InjectOptionalAttribute(object id = null) : base(id, true)
        { 
        }
    }
}
