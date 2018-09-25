//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntity.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Wiring;
    using Map;

    /// <summary>
    /// The total resource cost
    /// </summary>
    [Serializable]
    public class ResourceCost
    {
        public ResourceType Resource;
        public float Amount;
    }

    /// <summary>
    /// Defines an entity on the grid
    /// </summary>
    public abstract class GridEntity : MonoBehaviour
    {
        /// <summary>
        /// Gets the unique ID for the grid entity
        /// </summary>
        public int ID;

        /// <summary>
        /// The cost to manufacture the entity
        /// </summary>
        public List<ResourceCost> ManufactureCost;

        /// <summary>
        /// how many times this entity was rotated
        /// </summary>
        public int Rotation
        {
            get
            {
                return this._rotation;
            }
            set
            {
                while (value < 0)
                {
                    value += 4;
                }

                this._rotation = value % 4;
            }
        }

        /// <summary>
        /// The rotation of the GridEntity
        /// </summary>
        private int _rotation;

        /// <summary>
        /// The width of the entity
        /// </summary>
        public GridEntitySize Size;

        /// <summary>
        /// Rotates the object
        /// </summary>
        public virtual void Rotate(bool isClockWise)
        {
            this.Size = this.Size.Rotate(isClockWise);
            this.transform.localEulerAngles += new Vector3(0, 0, isClockWise ? -90 : 90);
            if (isClockWise)
            {
                this.Rotation++;
            }
            else
            {
                this.Rotation--;
            }
            this.OnMove();
        }

        /// <summary>
        /// Called when the entity moves to update the attached beams
        /// </summary>
        public abstract void OnMove();

        /// <summary>
        /// Generates a phantom of the grid entity for hovering
        /// </summary>
        /// <returns>The phantom</returns>
        public virtual GridEntity CreatePhantom()
        {
            var phantom = Instantiate(this.gameObject);

            // Set own color
            phantom.GetComponent<SpriteRenderer>().color = Settings.GeneralSettings.NormalPhantomColor;

            // Destroy children
            for (int i = 0; i < phantom.transform.childCount; i++)
            {
                var child = phantom.transform.GetChild(i);

                // If it's a socket, hide it 
                if (child.GetComponent<ISocket>() != null)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    var childSprite = child.GetComponent<SpriteRenderer>();
                    if (childSprite != null)
                    {
                        childSprite.color = Settings.GeneralSettings.NormalPhantomColor;
                    }
                }
            }

            return phantom.GetComponent<GridEntity>();
        }
    }
}
