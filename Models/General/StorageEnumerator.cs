using Helpers.General;
using Models.Contracts.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Models.General
{
    /// <summary>
    /// Enumerates storage and its sub storages
    /// </summary>
    public sealed class StorageEnumerator : IEnumerator<IStorage>
    {
        /// <summary>
        /// Root storage
        /// </summary>
        private readonly IStorage root;

        /// <summary>
        /// Stack to store items that are currently checked
        /// </summary>
        private readonly Stack<IStorage> stack = new();

        /// <summary>
        /// Ctor that accepts storage to enumerate
        /// </summary>
        public StorageEnumerator(IStorage root)
        {
            this.root = root;
            stack.Push(root);
        }

        public IStorage Current { get; private set; }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // TODO release managed resources here
        }

        public bool MoveNext()
        {
            bool canGoNext = false;

            if (stack.Count > 0)
            {
                canGoNext = true;
                Current = stack.Pop();

                var subDirectories = Current.EnumerateSubDirectories().ToArray();

                stack.PushRange(subDirectories);
            }

            return canGoNext;
        }

        public void Reset()
        {
            stack.Clear();
            stack.Push(root);
        }
    }
}
