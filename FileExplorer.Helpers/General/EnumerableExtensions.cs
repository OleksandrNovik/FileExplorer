#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Helpers.General
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Sorts <see cref="ParallelQuery" /> in provided order by provided property 
        /// </summary>
        /// <typeparam name="T"> Type of collection item </typeparam>
        /// <typeparam name="TKey"> Type of sort property </typeparam>
        /// <param name="query"> Collection to sort </param>
        /// <param name="sortFunc"> Sort property </param>
        /// <param name="isDescending"> True if sorting in descending order </param>
        /// <returns> Sorted collection </returns>
        public static OrderedParallelQuery<T> Sort<T, TKey>(this ParallelQuery<T> query, Func<T, TKey> sortFunc, bool isDescending)
        {
            return isDescending ? query.OrderByDescending(sortFunc) : query.OrderBy(sortFunc);
        }

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

        /// <summary>
        /// Adds all elements from source collection to a target collection
        /// </summary>
        /// <typeparam name="T"> Type of collection element </typeparam>
        /// <param name="target"> Target collection (we are adding items into) </param>
        /// <param name="source"> Source collection (from which items are copied to target) </param>
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

        /// <summary>
        /// Pushes items from list to the start of the stack 
        /// </summary>
        /// <typeparam name="T"> Type of collections element </typeparam>
        /// <param name="stack"> Target collection (for adding items) </param>
        /// <param name="source"> Source collection (for providing items) </param>
        public static void PushRange<T>(this Stack<T> stack, IList<T> source)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                stack.Push(source[i]);
            }
        }
    }
}
