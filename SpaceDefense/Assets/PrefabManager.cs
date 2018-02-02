//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="PrefabManager.cs">
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
    using Grid;

    /// <summary>
    /// Manages all prefabs
    /// </summary>
    public class PrefabManager : MonoBehaviour
    {
        /// <summary>
        /// A collection of prefabs for the entity 
        /// </summary>
        public List<GridEntity> EntityPrefabs;
        protected Dictionary<int, GridEntity> _entityHash = new Dictionary<int, GridEntity>();

        /// <summary>
        /// Gets the current instance  of the <see cref="PrefabManager"/> class
        /// </summary>
        public static PrefabManager CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = GameObject.FindGameObjectWithTag(Tags.PrefabManager).GetComponent<PrefabManager>();
                }

                return _currentInstance;

            }
        }
        private static PrefabManager _currentInstance;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            if (_currentInstance == null)
            {
                _currentInstance = this;
            }

            foreach (var entity in EntityPrefabs)
            {
                this._entityHash[entity.ID] = entity;
            }
        }

        /// <summary>
        /// Gets the grid entity prefab by their ID
        /// </summary>
        /// <param name="id">The target entity id</param>
        /// <returns>Prefab for the given entity id, null if invalid Id</returns>
        public GridEntity GetEntityPrefab(int id)
        {
            GridEntity result = null;
            if (!this._entityHash.TryGetValue(id, out result))
            {
                return null;
            }

            return result;
        }
    }
}
