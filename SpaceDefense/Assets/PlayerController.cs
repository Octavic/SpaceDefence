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
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                this._mouseDownSocket = this.GetSocketAtMousePosition();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var releasedOn = this.GetSocketAtMousePosition();

                if (releasedOn == null)
                {
                    this._mouseDownSocket = null;
                }
                else
                {
                    var output = this._mouseDownSocket as OutputSocket;
                    var input = releasedOn as InputSocket;

                    if (output != null && input != null)
                    {
                        output.TryAddInputSocket(input);
                        return;
                    }

                    output = releasedOn as OutputSocket;
                    input = this._mouseDownSocket as InputSocket;
                    if (output != null && input != null)
                    {
                        output.DisconnectInputSocket(input);
                    }
                }
            }
        }

        private ISocket GetSocketAtMousePosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.GetComponent<ISocket>();
            }

            return null;
        }

        private void ConnectSockets(OutputSocket source, InputSocket destination)
        {
            
        }

        private void DisconnectSockets(OutputSocket source, InputSocket destination)
        {

        }
    }
}
