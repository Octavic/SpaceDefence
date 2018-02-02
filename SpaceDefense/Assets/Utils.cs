//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Utils.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A collection of util functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Turns a key list and a value list into a dictionary
        /// </summary>
        /// <typeparam name="T">Type of key</typeparam>
        /// <typeparam name="G">Type of value</typeparam>
        /// <param name="keys">The list of keys</param>
        /// <param name="values">The list of values</param>
        /// <returns>A dictionary with key type of T and value type of G</returns>
        public static Dictionary<T, G> ConvertListToDictionary<T, G>(IList<T> keys, IList<G> values)
        {
            var keyCount = keys.Count;
            if (keyCount != values.Count)
            {
                throw new ArgumentException("Lists must be equal length");
            }

            var result = new Dictionary<T, G>();
            for (int i = 0; i < keyCount; i++)
            {
                result[keys[i]] = values[i];
            }

            return result;
        }
    }
}
