using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Jedi
{
    public partial class DiContainer
    {
        #region Properties

        /// <summary>
        /// All parents of this container
        /// </summary>
        private ConcurrentBag<DiContainer> _parents;

        /// <summary>
        /// All children containers of this container
        /// </summary>
        private ConcurrentBag<DiContainer> _children;

        /// <summary>
        /// A list of contracts bound to the container
        /// </summary>
        private ConcurrentDictionary<object, Contract> _contracts;

        #endregion

        #region ConstuctionAndLinkage

        /// <summary>
        /// Initiate a DiConainer with a list of children
        /// </summary>
        /// <param name="parents"></param>
        public DiContainer(params DiContainer[] parents)
        {
            _contracts = new ConcurrentDictionary<object, Contract>();
        }

        /// <summary>
        /// Add a DiContainer as a parent reference
        /// </summary>
        /// <param name="parent"></param>
        public void AddParent(DiContainer parent)
        {
            // Check if the parent bag is set
            if (_parents == null)
            {
                _parents = new ConcurrentBag<DiContainer>();
            }

            // Add to the parents bag
            _parents.Add(parent);

            // Add this as a child
            parent.AddChild(this);
        }

        /// <summary>
        /// Add a list of parents
        /// </summary>
        /// <param name="parents"></param>
        public void AddParents(params DiContainer[] parents)
        {
            // Add to parents
            foreach (DiContainer parent in parents)
            {
                AddParent(parent);
            }
        }

        /// <summary>
        /// Add a reference of a DiContainer as a child
        /// </summary>
        /// <param name="child"></param>
        private void AddChild(DiContainer child)
        {
            // Check if the child bag is set
            if (_children == null)
            {
                _children = new ConcurrentBag<DiContainer>();
            }

            // Add to the children bag
            _children.Add(child);
        }

        #endregion
    }
}
