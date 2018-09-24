//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerInventoryUI.cs">
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
    using Map;

    /// <summary>
    /// An UI element that shows what kind of resources the player own
    /// </summary>
    public class PlayerInventoryUI : MonoBehaviour
    {
        public List<PlayerInventoryResource> Resources;

        /// <summary>
        /// Gets the current instance of the <see cref="PlayerInventoryUI"/> class
        /// </summary>
        private static PlayerInventoryUI CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = GameObject.FindObjectOfType<PlayerInventoryUI>();
                }

                return _currentInstance;
            }
        }
        private static PlayerInventoryUI _currentInstance;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            if (_currentInstance != null && _currentInstance != this)
            {
                Destroy(this.gameObject);
                return;
            }

            _currentInstance = this;
        }

        /// <summary>
        /// Refreshes the UI
        /// </summary>
        public static void Refresh(IDictionary<ResourceType, float> amounts, IDictionary<ResourceType, float> capacities)
        {
            for (int i = 0; i < _currentInstance.Resources.Count; i++)
            {
                float amount = 0;
                float capacity = 0;
                var resourceType = (ResourceType)i;
                amounts.TryGetValue(resourceType, out amount);
                capacities.TryGetValue(resourceType, out capacity);

                _currentInstance.Resources[i].SetData(resourceType, amount, capacity);
            }
        }
    }
}
