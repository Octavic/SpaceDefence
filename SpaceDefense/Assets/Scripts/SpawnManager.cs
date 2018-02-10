﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SpawnManager.cs">
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

    /// <summary>
    /// Defines a path for an enemy
    /// </summary>
    [Serializable]
    public class Path
    {
        /// <summary>
        /// A collection of enemy type => time in between spawns
        /// </summary>
        public List<EnemyType> SpawnEnemies;
        public List<float> SpawnInterval;

        /// <summary>
        /// How long until the enemies spawns
        /// </summary>
        [HideInInspector]
        public IList<float> TimeUntilSpawn;
        
        /// <summary>
        /// Where the enemies spawn
        /// </summary>
        public Vector2 SpawnPos;

        /// <summary>
        /// A collection of nodes
        /// </summary>
        public List<Vector2> Nodes;
    }

    /// <summary>
    /// Controls enemy spawning, path, and etc
    /// </summary>
    public class SpawnManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="SpawnManager"/> class
        /// </summary>
        public static SpawnManager CurrntInstance { get; private set; }

        /// <summary>
        /// A list of paths
        /// </summary>
        public List<Path> Paths;

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            SpawnManager.CurrntInstance = this;
            for (int i = 0; i < this.Paths.Count; i++)
            {
                var path = this.Paths[i];
                if (path.SpawnEnemies.Count != path.SpawnInterval.Count)
                {
                    Debug.LogError("Invalid path in level manager");
                }
                else
                {
                    path.TimeUntilSpawn = new List<float>(path.SpawnInterval);
                }
            }
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            var timePassed = Time.deltaTime;

            var prefabManager = PrefabManager.CurrentInstance;

            // Update each path
            foreach (var path in this.Paths)
            {
                // Decay time
                var timeUntilSpawn = path.TimeUntilSpawn;
                for (int i = 0; i < timeUntilSpawn.Count; i++)
                {
                    timeUntilSpawn[i] -= timePassed;
                    if (timeUntilSpawn[i] < 0)
                    {
                        timeUntilSpawn[i] = path.SpawnInterval[i];

                        var newEnemyType = path.SpawnEnemies[i];
                        var newEnemyPrefab = prefabManager.GetEnemyPrefab(newEnemyType);
                        if (newEnemyPrefab == null)
                        {
                            Debug.LogError("Prefab for enemy not found: " + newEnemyType);
                        }
                        else
                        {
                            var newEnemy = Instantiate(newEnemyPrefab);
                            newEnemy.transform.position = path.SpawnPos;
                            newEnemy.Path = path.Nodes;
                        }
                    }
                }
            }
        }
    }
}