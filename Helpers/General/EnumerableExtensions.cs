﻿#nullable enable
using System.Collections;
using System.Collections.Generic;

namespace Helpers.General
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
        public static void RemoveRange<T>(this IList<T> target, IEnumerable<T> source)
        {
            foreach (var item in source)
            {
                target.Remove(item);
            }
        }

        public static void AppendFront<T>(this IList<T> target, IList<T>? source)
        {
            if (source is not null && source.Count > 0)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    target.Insert(i, source[i]);
                }
            }
        }

        public static void RemoveCollection<T>(this IList<T> target, ICollection<T>? source)
        {
            if (source is not null && source.Count > 0)
            {
                target.RemoveRange(source);
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
