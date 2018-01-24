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
    public class MapGrid : MonoBehaviour
    {
        /// <summary>
        /// Prefab for the grid cell
        /// </summary>
        public GameObject GridCellPrefab;
            
        /// <summary>
        /// Width of the grid
        /// </summary>
        public int SizeX;

        /// <summary>
        /// Height of the grid
        /// </summary>
        public int SizeY;

        /// <summary>
        /// Gets the current instance of the <see cref="MapGrid"/> class
        /// </summary>
        public static MapGrid CurrentInstance { get; private set; }

        /// <summary>
        /// The world position of the top left corner pixel  of the top left cell
        /// </summary>
        private Vector2 _topLeftCornerWorldPosition;

        /// <summary>
        /// A collection of coordinate => what is on that coordinate
        /// </summary>
        private Dictionary<GridCoordinate, GridEntity> _map = new Dictionary<GridCoordinate, GridEntity>();

        /// <summary>
        /// The gameobject containing all of the cells
        /// </summary>
        private GameObject _cells;

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
        /// Try to add the given entity
        /// </summary>
        /// <param name="newEntity">New entity to be added</param>
        /// <param name="mousePos">The mouse position in world space</param>
        /// <returns>True if the entity was added</returns>
        public bool TryAddEntity(GridEntity newEntity, Vector2 mousePos)
        {
            return this.TryAddEntity(newEntity, this.GetMouseHoveringCoordinate(mousePos));
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

            newEntity.transform.position = this.GetCellWorldPosition(coordinate);
            return true;
        }

        /// <summary>
        /// Gets the coordinate that the mouse is currently hovering over
        /// </summary>
        /// <param name="mousepos">The current mouse position in world space</param>
        /// <returns>The result</returns>
        public GridCoordinate GetMouseHoveringCoordinate(Vector2 mousepos)
        {
            var diff = mousepos - this._topLeftCornerWorldPosition;
            return new GridCoordinate((int)(diff.x / GeneralSettings.GridSize), (int)(diff.y / GeneralSettings.GridSize));
        }

        /// <summary>
        /// Gets the world position of the given cell
        /// </summary>
        /// <param name="coordinate">coordinate of the given cell</param>
        /// <returns>The world position of the cell</returns>
        public Vector2 GetCellWorldPosition(GridCoordinate coordinate)
        {
            return (Vector2)(this.transform.position) + coordinate.ToVector2() * GeneralSettings.GridSize;
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
        /// Constructs a list of needed coordinates
        /// </summary>
        /// <param name="newEntity">Target entity</param>
        /// <param name="index">Index coordinate</param>
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
            this._topLeftCornerWorldPosition = this.transform.position - new Vector3(GeneralSettings.GridSize / 2, GeneralSettings.GridSize / 2);
            var _boxCollider = this.GetComponent<BoxCollider2D>();
            _boxCollider.size = new Vector2(this.SizeX * GeneralSettings.GridSize, this.SizeY * GeneralSettings.GridSize);
            _boxCollider.offset = new Vector2((0.5f * this.SizeX - 0.5f) * GeneralSettings.GridSize, (0.5f * this.SizeY - 0.5f) * GeneralSettings.GridSize);

            var cells = new GameObject("Cells");
            cells.transform.parent = this.transform;
            cells.transform.localPosition = Vector3.zero;

            for (int x = 0; x < this.SizeX; x++)
            {
                for (int y = 0; y < this.SizeY; y++)
                {
                    var newGridCell = Instantiate(this.GridCellPrefab, cells.transform);
                    newGridCell.transform.localPosition = new Vector3(x, y) * GeneralSettings.GridSize;
                }
            }
            this._cells = cells;

            MapGrid.CurrentInstance = this;
        }
    }
}
