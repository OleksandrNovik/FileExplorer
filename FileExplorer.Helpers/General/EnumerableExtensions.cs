#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FileExplorer.Helpers.General
{
    public static class EnumerableExtensions
    {
        public static OrderedParallelQuery<T> Order<T, TKey>(this ParallelQuery<T> query, Func<T, TKey> sortFunc, bool isDescending)
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

        public static void AddRange<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Add(item);
            }
        }
        public static void RemoveRange<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Remove(item);
            }
        }

        public static void AppendFront<T>(this IList<T> target, IList<T> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                target.Insert(i, source[i]);
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

        public static void PushRange<T>(this Stack<T> stack, IList<T> source)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                stack.Push(source[i]);
            }
        }
    }
}
