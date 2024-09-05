using Models.Contracts.Storage;
using Models.Storage.Drives;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Models.General
{
    public sealed class DrivesCollectionEnumerator : IEnumerator<IStorage>
    {
        private List<StorageEnumerator> drivesEnumerators;
        private readonly List<StorageEnumerator> backup;
        private int counter;
        private int currentIndex;
        public IStorage Current { get; private set; }
        object IEnumerator.Current => Current;
        public DrivesCollectionEnumerator(ObservableDrivesCollection collection)
        {
            var drives = collection.EnumerateSubDirectories();
            drivesEnumerators = new List<StorageEnumerator>();

            foreach (var drive in drives)
            {
                drivesEnumerators.Add(new StorageEnumerator(drive));
            }

            backup = drivesEnumerators.ToList();
        }

        public void Dispose()
        {
            foreach (var enumerator in drivesEnumerators)
            {
                enumerator.Dispose();
            }
        }

        public bool MoveNext()
        {
            bool canGoNext = false;

            while (drivesEnumerators.Count > 0)
            {
                int index = (currentIndex + 1) % drivesEnumerators.Count;
                var currentEnumerator = drivesEnumerators[index];

                if (!currentEnumerator.MoveNext())
                {
                    drivesEnumerators.Remove(currentEnumerator);
                }
                else
                {
                    counter++;

                    if (counter == 50)
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
            drivesEnumerators = backup.ToList();
        }
    }
}
