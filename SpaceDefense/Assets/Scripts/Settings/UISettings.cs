//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="UISettings.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Utils;

    /// <summary>
    /// A collection of UI related settings
    /// </summary>
    public static class UISettings
    {
        /// <summary>
        /// The height of each map node info panel resource
        /// </summary>
        public const float MapNodeInfoResourceHeight = 50.0f;

        /// <summary>
        /// The color for an aced node
        /// </summary>

        public static Color AcedNodeColor = ColorExtension.FromInt(14, 240, 21);

        /// <summary>
        /// Color for a node that's available but not yet attempted
        /// </summary>
        public static Color AvailableNodeColor = ColorExtension.FromInt(255, 255, 255);

        /// <summary>
        /// Color of an unavailable map  node
        /// </summary>
        public static Color UnavailableNodeColor = ColorExtension.FromInt(103, 103, 103);

        /// <summary>
        /// Min and max for a node that's completed but not perfect (lerped by efficiency)
        /// </summary>
        public static Color MinImperfectNodeColor = ColorExtension.FromInt(206, 0, 0);
        public static Color MaxImprefectNodeColor = ColorExtension.FromInt(246, 239, 0);
    }
}
