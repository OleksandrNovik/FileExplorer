using Models.Contracts.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Models.Storage.Enumerators
{
    /// <summary>
    /// Enumerator that goes through each storage and sub storage in collection
    /// </summary>
    public sealed class StorageCollectionEnumerator : IEnumerator<IStorage>
    {
        /// <summary>
        /// List of <see cref="StorageEnumerator"/> for each collection item
        /// </summary>
        private List<StorageEnumerator> enumerators;

        /// <summary>
        /// Back up list to restore any time user calls <see cref="Reset"/> method 
        /// </summary>
        private readonly List<StorageEnumerator> backup;

        /// <summary>
        /// Operations counter
        /// </summary>
        private int counter;

        /// <summary>
        /// Current index of enumerator in list <see cref="enumerators"/>
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Current storage in enumeration
        /// </summary>
        public IStorage Current { get; private set; }
        object IEnumerator.Current => Current;

        /// <summary>
        /// Ctor that accepts collection to enumerate as parameter
        /// </summary>
        public StorageCollectionEnumerator(IEnumerable<IStorage> collection)
        {
            enumerators = new List<StorageEnumerator>();

            foreach (var drive in collection)
            {
                enumerators.Add(new StorageEnumerator(drive));
            }

            backup = enumerators.ToList();
        }

        public void Dispose()
        {
            foreach (var enumerator in enumerators)
            {
                enumerator.Dispose();
            }
        }

        /// <summary>
        /// Moves through all storages and sub storages in collection
        /// </summary>
        public bool MoveNext()
        {
            bool canGoNext = false;

            while (enumerators.Count > 0)
            {
                int index = (currentIndex + 1) % enumerators.Count;
                var currentEnumerator = enumerators[index];

                if (!currentEnumerator.MoveNext())
                {
                    enumerators.Remove(currentEnumerator);
                }
                else
                {
                    counter++;

                    if (counter == 10)
                    {
                        currentIndex++;
                        counter = 0;
                    }

                    Current = currentEnumerator.Current;
                    canGoNext = true;
                    break;
                }
            }

            return canGoNext;
        }

        public void Reset()
        {
            counter = currentIndex = 0;
            enumerators = backup.ToList();
        }
    }
}
