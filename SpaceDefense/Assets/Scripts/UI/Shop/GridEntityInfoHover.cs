//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntityInfoHover.cs">
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
    using Map.Grid;

    /// <summary>
    /// The information panel that pops up when the user hovers over a grid entity purchase button
    /// </summary>
    public class GridEntityInfoHover : MonoBehaviour
    {
        public static GridEntityInfoHover CurrentInstance { get; private set; }

        /// <summary>
        /// Button used to purchase the entity
        /// </summary>
        public Button PurchaseButton;

        /// <summary>
        /// The parent gameobject of the resource costs
        /// </summary>
        public GameObject ResourceCostParent;

        /// <summary>
        /// Prefab for the resource cost
        /// </summary>
        public EntityPurchaseResourceCost ResourceCostPrefab;

        /// <summary>
        /// The current entity that's being shown
        /// </summary>
        private GridEntity _showingEntityPrefab;

        /// <summary>
        /// Shows information about the given entity
        /// </summary>
        /// <param name="entityPrefab">Target entity</param>
        public void ShowEntity(GridEntity entityPrefab)
        {
            this._showingEntityPrefab = entityPrefab;
            this.PurchaseButton.interactable = SaveManager.CurrentInstance.CanAfford(entityPrefab.ManufactureCost);

            this.ResourceCostParent.DestroyAllChildren();
            for (int i = 0; i < entityPrefab.ManufactureCost.Count; i++)
            {
                var targetCost = entityPrefab.ManufactureCost[i];
                var newCostUI = Instantiate(this.ResourceCostPrefab, this.ResourceCostParent.transform);
                newCostUI.ShowResource(targetCost.Resource, targetCost.Amount);
                newCostUI.transform.localPosition = new Vector3(0, -i * Settings.UISettings.EntityInfoResourceHeight, 0);
            }
        }

        /// <summary>
        /// Called  when the entity was purchased
        /// </summary>
        public void OnPurchase()
        {
            var newEntity = Instantiate(this._showingEntityPrefab);
            newEntity.gameObject.SetActive(false);
            PlayerController.CurrentInstancce.OnCompletingPurchase(newEntity);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            CurrentInstance = this;
        }
    }
}
