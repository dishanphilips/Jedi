using System;
using System.Collections.Generic;
using System.Text;

namespace Jedi
{
    [AttributeUsage(AttributeTargets.Constructor|AttributeTargets.Field|AttributeTargets.Property|AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        /// The Id to be used when injecing
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// If true, no exceptions will be thrown when injecting
        /// </summary>
        public bool Optional { get; }


        /// <summary>
        /// Jedi injects when placed on a Constructor/Field/Property/Method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="optional"></param>
        public InjectAttribute(object id = null, bool optional = false)
        {
            Id = id;
            Optional = optional;
        }
    }
}
