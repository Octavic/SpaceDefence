//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SaveManager.cs">
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
    using UnityEngine.UI;

    /// <summary>
    /// Defines the shop menu tab that can be moved left => right
    /// </summary>
    public class ShopMenuTab : MonoBehaviour
    {
        /// <summary>
        /// Width of the tab, also how much to move by
        /// </summary>
        public float TabWidth;

        public Button LeftButton;
        public Button RightButton;

        /// <summary>
        /// All of the sub contents
        /// </summary>
        private List<ShopMenuTabContent> _contents;
        private int _contentCount;

        /// <summary>
        /// Index of the content that's currently being shown
        /// </summary>
        private int _currentShowingIndex;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._contents = this.transform.GetComponentsInChildren<ShopMenuTabContent>().ToList();
            this._contentCount = this._contents.Count;
            if (this._contents.Count == 0)
            {
                Debug.LogError("No content found for shop menu tab " + this.gameObject.name);
            }

            if (this._contents.Count == 1)
            {
                this.LeftButton.interactable = false;
                this.RightButton.interactable = false;
            }

            this._currentShowingIndex = 0;
        }

        public void Shift(int offset)
        {
            if (offset != 1 && offset != -1)
            {
                Debug.LogError("Shouldn't move more/less than 1");
            }

            var currentContent = this._contents[this._currentShowingIndex];
            this._currentShowingIndex += offset;
            if (this._currentShowingIndex < 0)
            {
                this._currentShowingIndex += this._contentCount;
            }
            this._currentShowingIndex = this._currentShowingIndex % this._contentCount;
            var newContent = this._contents[this._currentShowingIndex];

            currentContent.LerpTo(new Vector3(this.TabWidth * offset, 0));
            newContent.transform.localPosition = new Vector3(-this.TabWidth * offset, 0);
            newContent.LerpTo(new Vector3(0, 0));

        }
    }
}
