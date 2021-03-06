﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapGrid.cs">
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
    using Settings;
    using Wiring.Emitters;
    using Wiring;
    using States;
    using Utils;

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
        public int SizeX { get; private set; }

        /// <summary>
        /// Height of the grid
        /// </summary>
        public int SizeY { get; private set; }

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
        /// The game object containing all of the cells
        /// </summary>
        private GameObject _cells;

        /// <summary>
        /// The current instance  of the PrefabManager class
        /// </summary>
        private PrefabManager prefabManager;

        /// <summary>
        /// A stack of states that's been executed
        /// </summary>
        private LimitedStack<MapGridUndoItem> _undoStack = new LimitedStack<MapGridUndoItem>(GeneralSettings.GridUndoSteps);

        /// <summary>
        /// A stack of states that's been undid
        /// </summary>
        private LimitedStack<MapGridUndoItem> _redoStack = new LimitedStack<MapGridUndoItem>(GeneralSettings.GridUndoSteps);

        /// <summary>
        /// Called when the state of the grid changes
        /// </summary>
        public void OnStateChange(List<ResourceCost> costDiff = null)
        {
            var newState = this.SaveState();
            if (this._redoStack.Count > 0)
            {
                this._redoStack = new LimitedStack<MapGridUndoItem>(GeneralSettings.GridUndoSteps);
            }

            this._undoStack.Push(new MapGridUndoItem(newState, costDiff));
        }

        /// <summary>
        /// Undoes the last state
        /// </summary>
        public void Undo()
        {
            this.ResetBoard();
            this._redoStack.Push(this._undoStack.Pop());
            this._tryLoadFromState(this._undoStack.Peek().SaveState);
        }

        /// <summary>
        /// Redoes the last state
        /// </summary>
        public void Redo()
        {
            if (this._redoStack.Count > 0)
            {
                var item = this._redoStack.Pop();
                this._undoStack.Push(item);
                this.ResetBoard();
                this._tryLoadFromState(item.SaveState);
            }
        }

        public GridEntity GetEntityAtPosition(Vector2 mousePos)
        {
            var coordinate = this.GetMouseHoveringCoordinate(mousePos);
            return this.GetEntityAtPosition(coordinate);
        }
        public GridEntity GetEntityAtPosition(GridCoordinate coordinate)
        {
            GridEntity result = null;
            if (this._map.TryGetValue(coordinate, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Rotates the target entity
        /// </summary>
        /// <param name="targetEntity">Entity to be rotated</param>
        /// <param name="isClockWise">If the rotation is clockwise or not</param>
        public void RotateEntity(GridEntity targetEntity, bool isClockWise)
        {
            GridCoordinate index;
            if (!this._entities.TryGetValue(targetEntity, out index))
            {
                return;
            }

            var neededCoordinates = this._getNeededCoordinates(index, targetEntity.Size.Rotate(isClockWise));
            GridEntityContainer container;
            if (!this._canAddEntity(targetEntity, neededCoordinates, out container))
            {
                return;
            }

            targetEntity.Rotate(isClockWise);
            this.TryAddEntity(targetEntity, index);
        }

        /// <summary>
        /// See if the new entity can be added at the target coordinate (Using top left of entity as index)
        /// </summary>
        /// <param name="newEntity">new entity to be added</param>
        /// <param name="coordiante">target index coordinate</param>
        /// <returns>True if the entity can be added</returns>
        public bool CanAddEntity(GridEntity newEntity, GridCoordinate coordiante)
        {
            var neededCoordinates = _getNeededCoordinates(coordiante, newEntity.Size);
            GridEntityContainer c;
            return _canAddEntity(newEntity, neededCoordinates, out c);
        }

        /// <summary>
        /// Try to add the given entity
        /// </summary>
        /// <param name="newEntity">New entity to be added</param>
        /// <param name="mousePos">The mouse position in world space</param>
        /// <param name="addToUndoStack">If the action should be added to the save state</param>
        /// <returns>True if the entity was added</returns>
        public bool TryAddEntity(GridEntity newEntity, Vector2 mousePos, bool addToUndoStack = true)
        {
            return this.TryAddEntity(newEntity, this.GetMouseHoveringCoordinate(mousePos), addToUndoStack);
        }

        /// <summary>
        /// Removes the target entity from the grid. Does nothing if the item is not inside the grid
        /// </summary>
        /// <param name="targetEntity">Target entity</param>
        public void RemoveEntity(GridEntity targetEntity)
        {
            GridCoordinate indexCoor;
            if (!this._entities.TryGetValue(targetEntity, out indexCoor))
            {
                return;
            }

            var refundedAmount = SaveManager.CurrentInstance.SellItem(targetEntity);

            this._entities.Remove(targetEntity);
            var occupiedCoors = this._getNeededCoordinates(indexCoor, targetEntity.Size);
            foreach (var occupied in occupiedCoors)
            {
                this._map.Remove(occupied);
            }

            this.OnStateChange();
        }

        /// <summary>
        /// Try to add the given entity onto the map
        /// </summary>
        /// <param name="newEntity">The new entity to be added</param>
        /// <param name="coordinate">Target coordinate index</param>
        /// <param name="addToUndoStack">If the action should be added to the save state</param>
        /// <returns>True if operation succeed</returns>
        public bool TryAddEntity(GridEntity newEntity, GridCoordinate coordinate, bool addToUndoStack = true)
        {
            if(addToUndoStack && !SaveManager.CurrentInstance.TrySpendResource(newEntity.ManufactureCost))
            {
                return false;
            }

            var neededCoordinates = _getNeededCoordinates(coordinate, newEntity.Size);
            GridEntityContainer container;
            if (!this._canAddEntity(newEntity, neededCoordinates, out container))
            {
                return false;
            }

            if (container != null)
            {
                return container.TryAddEntity(newEntity);
            }
            else
            {
                foreach (var coor in neededCoordinates)
                {
                    this._map[coor] = newEntity;
                }

                newEntity.transform.position = this.GetCellWorldPosition(coordinate);
                this._entities[newEntity] = coordinate;

                if (addToUndoStack)
                {
                    this.OnStateChange(newEntity.ManufactureCost);
                }

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
        /// Saves the target entity into a state
        /// </summary>
        /// <param name="targetEntity">Target entity</param>
        /// <returns>Static state of the entity, null if unavailable</returns>
        private GridEntityState SaveEntityToState(GridEntity targetEntity)
        {
            GridCoordinate position;
            this._entities.TryGetValue(targetEntity, out position);
            if (position == null)
            {
                position = new GridCoordinate(-1, -1);
            }

            // Check if the target entity is a container
            var containerEntity = targetEntity as GridEntityContainer;
            GridEntityState entityState = null;

            if (containerEntity == null)
            {
                entityState = new GridEntityState();
            }
            else
            {
                var containerState = new GridEntityContainerState();

                if (containerEntity.CurrentlyHolding != null)
                {
                    var holdingEntityState = this.SaveEntityToState(containerEntity.CurrentlyHolding);
                    containerState.HoldingEntity = holdingEntityState;
                }

                entityState = containerState;
            }

            // Construct information about the entity

            entityState.EntityID = targetEntity.ID;
            entityState.PosX = position.X;
            entityState.PosY = position.Y;
            entityState.Rotation = targetEntity.Rotation;

            // Construct the connected outputs
            IEmitter emitter = targetEntity as IEmitter;
            if (emitter != null)
            {
                foreach (var output in emitter.Outputs)
                {
                    var outputState = new OutputSocketState();
                    foreach (var connection in output.ConnectedInputs)
                    {
                        var input = connection.Key;
                        var connectionState = new OutputConnectionState();
                        GridCoordinate pos = this.GetMouseHoveringCoordinate(input.transform.position);
                        GridEntity entityAtPos;

                        if (!this._map.TryGetValue(pos, out entityAtPos))
                        {
                            return null;
                        }

                        var receiver = entityAtPos as IReceiver;
                        if (receiver == null)
                        {
                            Debug.LogError("Item at " + pos.ToString() + " is not a receiver");
                            return null;
                        }

                        connectionState.ConnectedX = pos.X;
                        connectionState.ConnectedY = pos.Y;


                        connectionState.InputSocketIndex = receiver.IndexOf(input);

                        outputState.Connections.Add(connectionState);
                    }

                    entityState.Outputs.Add(outputState);
                }
            }

            return entityState;
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
            foreach (var entity in this._entities.Keys)
            {
                var entityState = this.SaveEntityToState(entity);
                if (entityState == null)
                {
                    return null;
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
        private GridEntity InstantiateEntityFromState(GridEntityState entityState)
        {
            var prefab = PrefabManager.CurrentInstance.GetEntityPrefab(entityState.EntityID);
            if (!prefab)
            {
                Debug.Log("Entity id not found: " + entityState.EntityID);
                this.ResetBoard();
                return null;
            }

            var newEntity = Instantiate(prefab);

            for (int i = 0; i < entityState.Rotation; i++)
            {
                newEntity.Rotate(true);
            }

            var containerState = entityState as GridEntityContainerState;
            if (containerState != null && containerState.HoldingEntity != null)
            {
                var containerdEntity = this.InstantiateEntityFromState(containerState.HoldingEntity);

                var newEntityContainer = newEntity as GridEntityContainer;
                if (newEntityContainer == null)
                {
                    Debug.LogError("Instance of state is not a container");
                    return null;
                }

                if (!newEntityContainer.TryAddEntity(containerdEntity))
                {
                    Debug.LogError("Failed to add contained entity to instantiated object");
                    return null;
                }
            }

            return newEntity;
        }

        public bool TryLoadFromState(MapGridState state)
        {
            return this._tryLoadFromState(state, true);
        }

        /// <summary>
        /// Try to load the grid from the given state
        /// </summary>
        /// <param name="state">Target state</param>
        /// <returns>True if operation succeed</returns>
        private bool _tryLoadFromState(MapGridState state, bool shouldSave = false)
        {
            if (state == null)
            {
                return false;
            }

            this.prefabManager = PrefabManager.CurrentInstance;

            if (state.SizeX != this.SizeX || state.SizeY != this.SizeY)
            {
                return false;
            }

            var stateToObject = new Dictionary<GridEntityState, GridEntity>();

            // First place all  of the entities
            foreach (var entityState in state.GridEntities)
            {
                // Try to instantiate the entities
                var entityObject = this.InstantiateEntityFromState(entityState);
                if (entityObject == null)
                {
                    Debug.Log("Failed to load entity at " + entityState.PosX + ',' + entityState.PosY);
                    return false;
                }
                else
                {
                    // Try to place the entity on board
                    var newCoor = new GridCoordinate(entityState.PosX, entityState.PosY);
                    if (!this.TryAddEntity(entityObject, newCoor, false))
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
                    var outputObj = emitter.GetOutputSocket(i);

                    foreach (var connection in outputState.Connections)
                    {
                        GridEntity connectedEntity;
                        var targetCoordinate = new GridCoordinate(connection.ConnectedX, connection.ConnectedY);
                        if (!this._map.TryGetValue(targetCoordinate, out connectedEntity))
                        {
                            Debug.Log("No entity at coordinate to connect to: " + targetCoordinate);
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

                        var targetInput = receiver.GetInputSocket(connection.InputSocketIndex);
                        if (!outputObj.TryAddInputSocket(targetInput))
                        {
                            Debug.Log("Failed to connect output socket to" + targetCoordinate);
                            this.ResetBoard();
                            return false;
                        }
                    }
                }
            }

            if (shouldSave)
            {
                this.OnStateChange();
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
                        if (checkContainer.CanAddEntity(newEntity))
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
        /// <param name="targetEntity">Target entity</param>
        /// <param name="index">Index coordinate</param>
        /// <returns>A list of coordinates that the target entity will take up</returns>
        private List<GridCoordinate> _getNeededCoordinates(GridCoordinate index, GridEntitySize entitySize)
        {
            List<GridCoordinate> result = new List<GridCoordinate>();

            int signX = entitySize.ExtrudeX > 0 ? 1 : -1;
            int signY = entitySize.ExtrudeY > 0 ? 1 : -1;

            for (int x = 0; x <= Math.Abs(entitySize.ExtrudeX); x++)
            {
                for (int y = 0; y <= Math.Abs(entitySize.ExtrudeY); y++)
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
            if (LevelManager.CurrentInstance.CurrentLevel == null)
            {
                Debug.LogError("No level selected!");
                return;
            }

            var levelData = LevelManager.CurrentInstance.CurrentLevel.LevelData;
            this.SizeX = levelData.GridSizeX;
            this.SizeY = levelData.GridSizeY;

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

            // Try to load from state
            this.TryLoadFromState(LevelManager.CurrentInstance.CurrentLevel.SaveData.SavedState);
        }
    }
}
