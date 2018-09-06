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
    using Map.Grid;

    /// <summary>
    /// The actual click-able purchasable item
    /// </summary>
    public class ShopMenuItem : MonoBehaviour
    {
        /// <summary>
        /// The highlight object 
        /// </summary>
        public GameObject SelectHighlight;
        
        /// <summary>
        /// The item that this shop menu is selling
        /// </summary>
        public GridEntity TargetItem;

        /// <summary>
        /// The rotation of the button as well as resulting object
        /// </summary>
        private int Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                this.transform.localEulerAngles = new Vector3(0, 0, value * 90);
                this._rotation = value;
            }
        }
        private int _rotation;

        /// <summary>
        /// Called when the item is clicked
        /// </summary>
        public void OnSelect()
        {
            this.Rotation = 0;
        }

        /// <summary>
        /// Called when some other item is clicked
        /// </summary>
        public void OnDeselect()
        {
            this.Rotation = 0;
        }
    }
}
