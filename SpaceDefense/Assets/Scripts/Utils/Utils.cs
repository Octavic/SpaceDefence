//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Utils.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Map.Wiring;

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

        /// <summary>
        /// Updates all of the beams connected to the given inputs
        /// </summary>
        /// <param name="inputs">Target inputs</param>
        public static void UpdateAllBeams(IList<InputSocket> inputs)
        {
            foreach (var input in inputs)
            {
                foreach (var connectedOutput in input.ConnectedOutputs)
                {
                    connectedOutput.Key.UpdateBeam(input);
                }
            }
        }

        /// <summary>
        /// Updates all of the beams connected to the given outputs
        /// </summary>
        /// <param name="outputs">Target ouputs</param>
        public static void UpdateAllBeams(IList<OutputSocket> outputs)
        {
            foreach (var output in outputs)
            {
                output.UpdateBeam();
            }
        }

        public static float Lerp(float f1, float f2, float ratio)
        {
            return (f2 - f1) * ratio + f1;
        }
    }
}
