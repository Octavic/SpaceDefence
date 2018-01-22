//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerController.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;
    using Wiring.Emitters;

    /// <summary>
    /// The player controller class
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// The socket clicked on when the mouse was down
        /// </summary>
        private ISocket _mouseDownSocket;

        /// <summary>
        /// handles all mouse actions
        /// </summary>
        private void HandleMouse()
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
                            return;
                        }

                        output = hoveringSocket as OutputSocket;
                        input = this._mouseDownSocket as InputSocket;
                        if (output != null && input != null)
                        {
                            output.DisconnectInputSocket(input);
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
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            this.HandleMouse();
        }
    }
}
