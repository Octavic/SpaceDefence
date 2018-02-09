//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Grid.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;
    using Wiring.Emitters;
    using Wiring;
    using States;

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
        private Vector2 _bottomLeftWorldPosition;

        /// <summary>
        /// A collection of coordinate => what is on that coordinate
        /// </summary>
        private Dictionary<GridCoordinate, GridEntity> _map = new Dictionary<GridCoordinate, GridEntity>();

        /// <summary>
        /// A collection of grid entity => their index coordinate
        /// </summary>
        private Dictionary<GridEntity, GridCoordinate> _entities = new Dictionary<GridEntity, GridCoordinate>();

        /// <summary>
        /// The gameobject containing all of the cells
        /// </summary>
        private GameObject _cells;

        /// <summary>
        /// The current prefab manager instance
        /// </summary>
        private PrefabManager prefabs = PrefabManager.CurrentInstance;
        
        /// <summary>
        /// See if the new entity can be added at the target coordinate (Using top left of entity as index)
        /// </summary>
        /// <param name="newEntity">new entity to be added</param>
        /// <param name="coordiante">target index coordinate</param>
        /// <returns>True if the entity can be added</returns>
        public bool CanAddEntity(GridEntity newEntity, GridCoordinate coordiante)
        {
            var neededCoordinates = _getNeededCoordinates(newEntity, coordiante);
            GridEntityContainer c;
            return _canAddEntity(newEntity, neededCoordinates, out c);
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
            GridEntityContainer container;
            if (!this._canAddEntity(newEntity, neededCoordinates, out container))
            {
                return false;
            }

            if (container != null)
            {
                if (!container.TryAddEntity(newEntity))
                {
                    Debug.Log("Error when adding entity to container");
                    return false;
                }

                return true;
            }
            else
            {
                foreach (var coor in neededCoordinates)
                {
                    this._map[coor] = newEntity;
                }

                newEntity.transform.position = this.GetCellWorldPosition(coordinate);
                this._entities[newEntity] = coordinate;
                return true;
            }
        }

        /// <summary>
        /// Gets the coordinate that the mouse is currently hovering over
        /// </summary>
        /// <param name="mousepos">The current mouse position in world space</param>
        /// <returns>The result</returns>
        public GridCoordinate GetMouseHoveringCoordinate(Vector2 mousepos)
        {
            var diff = mousepos - this._bottomLeftWorldPosition;
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
        /// Saves the current state of the board into a map state
        /// </summary>
        /// <returns>The resulting mapgrid state, null if failed</returns>
        public MapGridState SaveState()
        {
            var result = new MapGridState();
            result.SizeX = this.SizeX;
            result.SizeY = this.SizeY;
            foreach (var item in this._entities)
            {
                var entity = item.Key;
                var position = item.Value;

                // Construct information about the entity
                var entityState = new GridEntityState();
                entityState.EntityID = entity.ID;
                entityState.PosX = position.X;
                entityState.PosY = position.Y;
                entityState.Rotation = entity.Rotation;

                // Construct the connected outputs
                IEmitter emitter = entity as IEmitter;
                if (emitter != null)
                {
                    foreach (var output in emitter.Outputs)
                    {
                        var outputState = new OutputSocketState();
                        foreach (var connection in output.ConnectedInputs)
                        {
                            var input = connection.Key;
                            var inputSource = input.Receiver;
                            var connectionState = new OutputConnectionState();
                            GridCoordinate pos;
                            if (!this._entities.TryGetValue(inputSource as GridEntity, out pos))
                            {
                                Debug.LogWarning("Output connected to item not registered on map");
                                return null;
                            }

                            connectionState.ConnectedX = pos.X;
                            connectionState.ConnectedY = pos.Y;
                            connectionState.InputSocketIndex = inputSource.Inputs.IndexOf(input);

                            outputState.Connections.Add(connectionState);
                        }

                        entityState.Outputs.Add(outputState);
                    }
                }

                result.GridEntities.Add(entityState);
            }

            return result;
        }

        /// <summary>
        /// Try to load an entity from state
        /// </summary>
        /// <param name="entityState">Target entity state</param>
        /// <returns>The resulting entity object</returns>
        private GridEntity TryInstantiateEntityFromState(GridEntityState entityState)
        {
            var prefab = prefabs.GetEntityPrefab(entityState.EntityID);
            if (!prefab)
            {
                Debug.Log("Entity id not found: " + entityState.EntityID);
                this.ResetBoard();
                return null;
            }

            var newEntity = Instantiate(prefab);

            for (int i = 0; i < entityState.Rotation; i++)
            {
                newEntity.RotateClockwise();
            }

            return newEntity;
        }

        /// <summary>
        /// Try to load the grid from the given state
        /// </summary>
        /// <param name="state">Target state</param>
        /// <returns>True if operation succeed</returns>
        public bool TryLoadFromState(MapGridState state)
        {
            if (state.SizeX != this.SizeX || state.SizeY != this.SizeY)
            {
                return false;
            }

            var stateToObject = new Dictionary<GridEntityState, GridEntity>();

            // First place all  of the entities
            foreach (var entityState in state.GridEntities)
            {
                // Try to instantiate the entities
                var entityObject = this.TryInstantiateEntityFromState(entityState);
                if (entityObject == null)
                {
                    return false;
                }
                else 
                {
                    // Try to place the entity on board
                    var newCoor = new GridCoordinate(entityState.PosX, entityState.PosY);
                    if (!this.TryAddEntity(entityObject, newCoor))
                    {
                        Debug.Log("Failed to add entity at " + newCoor);
                        this.ResetBoard();
                        return false;
                    }
                }

                // Hash result for the connecting part
                stateToObject[entityState] = entityObject;
            }

            // Then connect all of the outputs to input
            foreach (var entity in state.GridEntities)
            {
                var emitter = stateToObject[entity] as IEmitter;
                for (int i = 0; i < entity.Outputs.Count; i++)
                {
                    var outputState = entity.Outputs[i];
                    var outputObj = emitter.Outputs[i];

                    foreach (var connection in outputState.Connections)
                    {
                        GridEntity connectedEntity;
                        var targetCoordinate = new GridCoordinate(connection.ConnectedX, connection.ConnectedY);
                        if (!this._map.TryGetValue(targetCoordinate, out connectedEntity))
                        {
                            Debug.Log("No entity at coordinate to connect to: "+ targetCoordinate);
                            this.ResetBoard();
                            return false;
                        }

                        var receiver = connectedEntity as IReceiver;
                        if (receiver == null)
                        {
                            Debug.Log("Entity at coordinate is not a receiver: " + targetCoordinate);
                            this.ResetBoard();
                            return false;
                        }

                        var targetInput = receiver.Inputs[connection.InputSocketIndex];
                        if (!outputObj.TryAddInputSocket(targetInput))
                        {
                            Debug.Log("Failed to connect output socket to" + targetCoordinate);
                            this.ResetBoard();
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Reset the board
        /// </summary>
        public void ResetBoard()
        {
            foreach (var entity in this._entities)
            {
                Destroy(entity.Key.gameObject);
            }

            this._entities = new Dictionary<GridEntity, GridCoordinate>();
            this._map = new Dictionary<GridCoordinate, GridEntity>();
        }

        /// <summary>
        /// Calculates if a new entity can be added
        /// </summary>
        /// <param name="newEntity"></param>
        /// <param name="neededCoordinates">All of the coordinates that the new entity will take up</param>
        /// <returns>True if possible</returns>
        private bool _canAddEntity(GridEntity newEntity, List<GridCoordinate> neededCoordinates, out GridEntityContainer container)
        {
            container = null;

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
                    var checkContainer = occupied as GridEntityContainer;
                    if (checkContainer)
                    {
                        if (container.TryAddEntity(newEntity))
                        {
                            container = checkContainer;
                            return true;
                        }

                        return false;
                    }
                    else
                    {
                        return false;
                    }
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

            int signX = newEntity.ExtrudeX > 0 ? 1 : -1;
            int signY = newEntity.ExtrudeY > 0 ? 1 : -1;

            for (int x = 0; x <= Math.Abs(newEntity.ExtrudeX); x++)
            {
                for (int y = 0; y <= Math.Abs(newEntity.ExtrudeY); y++)
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
            this._bottomLeftWorldPosition = this.transform.position - new Vector3(GeneralSettings.GridSize / 2, GeneralSettings.GridSize / 2);
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
