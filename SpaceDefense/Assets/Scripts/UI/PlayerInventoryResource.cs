//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayInventoryResourcec.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
    using Map;

    /// <summary>
    /// Shows the currently owned amount and the capacity of a player inventory resource
    /// </summary>
    public class PlayerInventoryResource : MonoBehaviour
    {
        public Image ResourceIcon;
        public Text ResourceAmount;
        public Text ResourceCapacity;

        public void SetData(ResourceType resource, float amount, float capacity)
        {
            this.ResourceIcon.sprite = PrefabManager.CurrentInstance.GetResourceSprite(resource);
            this.ResourceAmount.text = ((int)(amount)).ToString();
            this.ResourceCapacity.text = ((int)(capacity)).ToString();
        }
    }
}
