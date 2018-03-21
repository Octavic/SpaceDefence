//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ShopMenuItem.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Grid;

    /// <summary>
    /// The actual click-able purchasable item
    /// </summary>
    public class ShopMenuItem : MonoBehaviour
    {
        /// <summary>
        /// The item that this shop menu is selling
        /// </summary>
        public GridEntity TargetItem;
    }
}
