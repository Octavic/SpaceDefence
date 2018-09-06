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
    using UnityEngine.UI;
    using Map.Wiring;
    using Map.Wiring.Emitters;
    using Map.Grid;

    /// <summary>
    /// The player controller class
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The parent gameobject that holds all entity modify buttons
        /// </summary>
        public GameObject EntityModifyButtonParent;

        /// <summary>
        /// A list of buttons used to modify the entity
        /// </summary>
        private List<Button> _entityModifyButtons = new List<Button>();

        /// <summary>
        /// The hover beam object
        /// </summary>
        public AttachableBeam HoverBeam;

        /// <summary>
        /// Gets the current instance of the <see cref="PlayerController"/> class
        /// </summary>
        public static PlayerController CurrentInstancce { get; private set; }

        /// <summary>
        /// The grid entity that's being held right now
        /// </summary>
        private GridEntity HoldingEntity
        {
            get
            {
                return this._holdingEntity;
            }
            set
            {
                foreach (var button in this._entityModifyButtons)
                {
                    if (button != null)
                    {
                        button.interactable = (value != null);
                    }
                }

                this._holdingEntity = value;
            }
        }
        private GridEntity _holdingEntity;
        private bool _isHeldOnGrid;
        private bool _isMoving;

        /// <summary>
        /// The phantom object that resembles the held entity
        /// </summary>
        private GridEntity HoldingPhantom;

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

            // Get all clicked sockets
            RaycastHit2D[] hits = null;
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mouseDown || mouseUp)
            {
                hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 15);
                if (hits.Length == 0)
                {
                    return;
                }
            }

            // If mouse is held, change the hover beam
            if (this._mouseDownSocket != null)
            {
                var socketObj = this._mouseDownSocket as MonoBehaviour;
                if (socketObj != null)
                {
                    this.HoverBeam.Attach(socketObj.transform.position, mousePos);
                }
            }

            var hoveringSocket = GetFirstHitWithComponent<ISocket>(hits);

            if (hoveringSocket != null)
            {
                if (mouseDown)
                {
                    this._mouseDownSocket = hoveringSocket;
                    this.HoverBeam.gameObject.SetActive(true);
                }
                else if (mouseUp)
                {
                    this.HoverBeam.gameObject.SetActive(false);
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
        }

        /// <summary>
        /// Handles all entity placements
        /// </summary>
        private void HandleEntityInteraction()
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

            // Handles  phantom movement
            if (this.HoldingPhantom != null)
            {
                this.HoldingPhantom.transform.position = mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                // If the mouse indeed clicked a grid
                if (grid != null)
                {
                    // If the player is actually holding onto something
                    if (this.HoldingEntity != null && this._isMoving)
                    {
                        if (grid.TryAddEntity(this.HoldingEntity, mousePos))
                        {
                            this.HoldingEntity.gameObject.SetActive(true);
                            this.HoldingEntity.OnMove();

                            if (this.HoldingPhantom != null)
                            {
                                Destroy(this.HoldingPhantom.transform.gameObject);
                            }

                            this._isMoving = false;
                            this.HoldingEntity = null;
                        }
                    }
                    else
                    {
                        // Not holding anything, instead pick up the item that was placed there
                        this.HoldingEntity = grid.GetEntityAtPosition(mousePos);
                        this._isHeldOnGrid = true;
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
            if (this.HoldingEntity != null && !this._isHeldOnGrid)
            {
                Destroy(this.HoldingEntity.gameObject);

                if (this.HoldingPhantom != null)
                {
                    Destroy(this.HoldingPhantom.gameObject);
                }
            }

            this.HoldingEntity = purchased;
            this.HoldingPhantom = this.HoldingEntity.CreatePhantom();
            this.HoldingPhantom.gameObject.SetActive(true);
            this._isMoving = true;
            this._isHeldOnGrid = false;
        }

        /// <summary>
        /// Deletes the item that's being held 
        /// </summary>
        public void RemoveHolding()
        {
            if (this.HoldingEntity == null)
            {
                return;
            }

            MapGrid.CurrentInstance.RemoveEntity(this.HoldingEntity);
            Destroy(this.HoldingEntity.gameObject);
            this.HoldingEntity = null;

            if (this.HoldingPhantom != null)
            {
                Destroy(this.HoldingPhantom.gameObject);
                this.HoldingPhantom = null;
            }
        }

        /// <summary>
        /// Rotates the holding item
        /// </summary>
        public void RotateHolding(bool isClockwise)
        {
            if (this.HoldingEntity != null)
            {
                if (this._isHeldOnGrid)
                {
                    MapGrid.CurrentInstance.RotateEntity(this.HoldingEntity, isClockwise);
                }
                else
                {
                    this.HoldingEntity.Rotate(isClockwise);
                }

                if (this.HoldingPhantom != null)
                {
                    this.HoldingPhantom.Rotate(isClockwise);
                }
            }
        }

        /// <summary>
        ///  Turn on the move mode for the item that's held
        /// </summary>
        public void MoveHolding()
        {
            if (this.HoldingEntity == null)
            {
                return;
            }

            if (this.HoldingPhantom != null)
            {
                Destroy(this.HoldingPhantom.gameObject);
            }

            this.HoldingPhantom = this.HoldingEntity.CreatePhantom();
            this._isMoving = true;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (GameController.CurrentInstance.CurrentPhasee != GamePhases.Build)
            {
                return;
            }

            this.HandleEntityInteraction();
            this.HandleSocketInteraction();
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            PlayerController.CurrentInstancce = this;

            this._isMoving = false;
            this._entityModifyButtons = this.EntityModifyButtonParent.GetComponentsInChildren<Button>().ToList();
        }
    }
}
