//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ShopMenu.cs">
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
    /// The shop menu 
    /// </summary>
    public class ShopMenu : MonoBehaviour
    {
        /// <summary>
        /// A collection of tab contents
        /// </summary>
        public List<GameObject> TabContents;

        /// <summary>
        /// The current instance of the <see cref="ShopMenu"/> class
        /// </summary>
        public static ShopMenu CurrentInstance { get; private set; }

        /// <summary>
        /// The currently selected grid entity
        /// </summary>
        public ShopMenuItem CurrentlySelected { get; private set; }

        /// <summary>
        /// Switches  to the given tab
        /// </summary>
        /// <param name="targetTabType">Target type</param>
        public void SwitchTabs(int targetTabType)
        {
            for(int i =0;i<this.TabContents.Count;i++)
            {
                this.TabContents[i].gameObject.SetActive(i == targetTabType);
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            ShopMenu.CurrentInstance = this;
        }
    }
}
