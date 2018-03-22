//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerController.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;
    using Wiring.Emitters;
    using Grid;

    /// <summary>
    /// The player controller class
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="PlayerController"/> class
        /// </summary>
        public static PlayerController CurrentInstancce { get; private set; }

        /// <summary>
        /// If the player can purchase things right now
        /// </summary>
        public static bool CanPurchase
        {
            get
            {
                return PlayerController.CurrentInstancce._holdingGridEntity == null;
            }
        }

        /// <summary>
        /// The grid entity that's being held right now
        /// </summary>
        private GridEntity _holdingGridEntity;

        /// <summary>
        /// The socket clicked on when the mouse was down
        /// </summary>
        private ISocket _mouseDownSocket;

        /// <summary>
        /// handles all socket interactions
        /// </summary>
        private void HandleSocketInteraction()
        {
            var mouseDown = Input.GetMouseButtonDown(0);
            var mouseUp = Input.GetMouseButtonUp(0);

            RaycastHit2D[] hits = null;
            if (mouseDown || mouseUp)
            {
                hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 15);
                if (hits.Length == 0)
                {
                    return;
                }
            }

            
            #region Socket interactions
            var hoveringSocket = GetFirstHitWithComponent<ISocket>(hits);

            if (hoveringSocket != null)
            {
                if (mouseDown)
                {
                    this._mouseDownSocket = hoveringSocket;
                }
                else if (mouseUp)
                {
                    if (hoveringSocket != null)
                    {
                        var output = this._mouseDownSocket as OutputSocket;
                        var input = hoveringSocket as InputSocket;

                        if (output != null && input != null)
                        {
                            output.TryAddInputSocket(input);
                            MapGrid.CurrentInstance.OnStateChange();
                            return;
                        }

                        output = hoveringSocket as OutputSocket;
                        input = this._mouseDownSocket as InputSocket;
                        if (output != null && input != null)
                        {
                            output.DisconnectInputSocket(input);
                            MapGrid.CurrentInstance.OnStateChange();
                        }
                    }

                    this._mouseDownSocket = null;
                }
            }
            #endregion

            #region Manual control interaction

            if (mouseDown)
            {
                var hoverControl = hoveringSocket != null ? null : GetFirstHitWithComponent<ManualControl>(hits);
                if (hoverControl != null)
                {
                    hoverControl.OnClick();
                }
            }
            #endregion
        }

        /// <summary>
        /// Handles all entity placements
        /// </summary>
        private void HandlePlaceEntity()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 15);
                if (hits.Length == 0)
                {
                    return;
                }

                MapGrid grid = null;
                Vector2 mousePos = new Vector2();
                foreach (var hit in hits)
                {
                    var tryGrid = hit.collider.gameObject.GetComponent<MapGrid>();
                    if (tryGrid)
                    {
                        grid = tryGrid;
                        mousePos = hit.point;
                    }
                }

                if (grid != null)
                {
                    if (grid.TryAddEntity(this._holdingGridEntity, mousePos))
                    {
                        this._holdingGridEntity = null;
                    }
                }
            }
        }

        /// <summary>
        /// Get the item in the list of hits from a raycast that contains the target component
        /// </summary>
        /// <typeparam name="T">Target component</typeparam>
        /// <param name="hits">List of raycast hits</param>
        /// <returns>The first component hit</returns>
        private static T GetFirstHitWithComponent<T>(RaycastHit2D[] hits)
        {
            if (hits == null || hits.Length == 0)
            {
                return default(T);
            }

            foreach (var hit in hits)
            {
                var component = hit.collider.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Called when a purchase is complete
        /// </summary>
        /// <param name="purchased">The item purchased</param>
        public void OnCompletingPurchase(GridEntity purchased)
        {
            this._holdingGridEntity = purchased;
        }

        /// <summary>
        /// Rotates the holding item
        /// </summary>
        public void RotateHolding(bool clockwise)
        {
            if (this._holdingGridEntity != null)
            {
                if (clockwise)
                {
                    this._holdingGridEntity.RotateClockwise();
                }
                else
                {
                    this._holdingGridEntity.RotateCounterClockwise();
                }
            }
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (this._holdingGridEntity != null)
            {
                this.HandlePlaceEntity();
            }
            else
            {
                this.HandleSocketInteraction();
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            PlayerController.CurrentInstancce = this;
        }
    }
}
