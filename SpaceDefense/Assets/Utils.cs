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

    /// <summary>
    /// A collection of util functions
    /// </summary>
    public static class Utils
    {
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
