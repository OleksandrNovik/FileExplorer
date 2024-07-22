using System.Collections;
using System.Collections.Generic;

namespace FileExplorer.Helpers
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Adds all elements from source collection to a target collection
        /// </summary>
        /// <param name="target"> Target collection (we are adding items into) </param>
        /// <param name="source"> Source collection (from which items are copied to target) </param>
        public static void AddRange(this IList target, IEnumerable source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        public static void AddRange<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }

        /// <summary>
        /// Removes all elements from source collection to a target collection
        /// </summary>
        /// <param name="target"> Target collection (we are adding items into) </param>
        /// <param name="source"> Source collection (from which items are copied to target) </param>
        public static void RemoveRange(this IList target, IEnumerable source)
        {
            foreach (var item in source)
            {
                target.Remove(item);
            }
        }
    }
}
