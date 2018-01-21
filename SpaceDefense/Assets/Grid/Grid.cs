//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Grid.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;

    /// <summary>
    /// A grid for the big map
    /// </summary>
    public class Grid : MonoBehaviour
    {
        /// <summary>
        /// Width of the grid
        /// </summary>
        public int SizeX;

        /// <summary>
        /// Height of the grid
        /// </summary>
        public int SizeY;

        /// <summary>
        /// The world position of the top left corner pixel  of the top left cell
        /// </summary>
        private Vector2 _topLeftCornerWorldPosition;

        /// <summary>
        /// A collection of coordinate => what is on that coordinate
        /// </summary>
        private Dictionary<GridCoordinate, GridEntity> _map = new Dictionary<GridCoordinate, GridEntity>();

        /// <summary>
        /// See if the new entity can be added at the target coordinate (Using top left of entity as index)
        /// </summary>
        /// <param name="newEntity">new entity to be added</param>
        /// <param name="coordiante">target index coordinate</param>
        /// <returns>True if the entity can be added</returns>
        public bool CanAddEntity(GridEntity newEntity, GridCoordinate coordiante)
        {
            var neededCoordinates = _getNeededCoordinates(newEntity, coordiante);
            return _canAddEntity(newEntity, neededCoordinates);
        }

        /// <summary>
        /// Try to add the given entity onto the map
        /// </summary>
        /// <param name="newEntity">The new entity to be added</param>
        /// <param name="coordinate">Target coordinate index</param>
        /// <returns>True if operation succeed</returns>
        public bool TryAddEntity(GridEntity newEntity, GridCoordinate coordinate)
        {
            var neededCoordinates = _getNeededCoordinates(newEntity, coordinate);
            if (!this._canAddEntity(newEntity, neededCoordinates))
            {
                return false;
            }

            foreach (var coor in neededCoordinates)
            {
                this._map[coor] = newEntity;
            }

            return true;
        }

        /// <summary>
        /// Gets the coordinate that the mouse is currently hovering over
        /// </summary>
        /// <param name="mousepos">The current mouse pos in world space</param>
        /// <returns>The result</returns>
        public GridCoordinate GetMouseHoveringCoordinate(Vector2 mousepos)
        {
            var diff = mousepos - this._topLeftCornerWorldPosition;
            return new GridCoordinate((int)(diff.x / GeneralSettings.GridSize), (int)(diff.y / GeneralSettings.GridSize));
        }

        /// <summary>
        /// Calculates if a new entity can be added
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="neededCoordinates">All of the coordinates that the new entity will take up</param>
        /// <returns>True if possible</returns>
        private bool _canAddEntity(GridEntity newEntity, List<GridCoordinate> neededCoordinates)
        {
            foreach (var checkCoor in neededCoordinates)
            {
                // If a space that needs to be available is outside the map
                if (checkCoor.X < 0 || checkCoor.Y < 0 || checkCoor.X >= this.SizeX || checkCoor.Y >= this.SizeY)
                {
                    return false;
                }

                GridEntity occupied;

                if (this._map.TryGetValue(checkCoor, out occupied) && occupied != newEntity)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Construsts a list of needed coordinates
        /// </summary>
        /// <param name="newEntity">Target entity</param>
        /// <param name="index">Index coordiante</param>
        /// <returns>A list of coordinates that the target entity will take up</returns>
        private List<GridCoordinate> _getNeededCoordinates(GridEntity newEntity, GridCoordinate index)
        {
            List<GridCoordinate> result = new List<GridCoordinate>();
            if (newEntity.SizeOffsetX == 0 || newEntity.SizeOffsetY == 0)
            {
                return result;
            }

            int signX = newEntity.SizeOffsetX > 0 ? 1 : -1;
            int signY = newEntity.SizeOffsetY > 0 ? 1 : -1;

            for (int x = 0; x < Math.Abs( newEntity.SizeOffsetX); x+=signX)
            {
                for (int y = 0; y < Math.Abs( newEntity.SizeOffsetY); y+=signY)
                {
                    result.Add(index + new GridCoordinate(x * signX, y * signY));
                }
            }
            return result;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._topLeftCornerWorldPosition = this.transform.position - new Vector3(this.SizeX, this.SizeY) / 2 * GeneralSettings.GridSize;
        }
    }
}
