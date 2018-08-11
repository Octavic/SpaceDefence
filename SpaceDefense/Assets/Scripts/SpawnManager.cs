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
    using Settings;
    using Enemies;
    
    [Serializable]
    public class PathRegularSpawn
    {
        public EnemyType Enemy;
        public float SpawnInterval;
        [HideInInspector]
        public float TimeUntilSpawn;
    }

    public class PathSpecialSpawn
    {
        public EnemyType Enemy;
        public float SpecialSpawnTime;
    }

    /// <summary>
    /// Defines a path for an enemy
    /// </summary>
    [Serializable]
    public class Path
    {
        /// <summary>
        /// A collection of enemy type => time in between spawns
        /// </summary>
        public List<PathRegularSpawn> RegularSpawns;

        /// <summary>
        /// How many seconds before the game ends, should the path stop spawning enemies
        /// </summary>
        public float StopSpawnBeforeEnd;

        /// <summary>
        /// A list of special designed enemies and when they'll spawn
        /// </summary>
        public List<PathSpecialSpawn> SpecialSpawns;

        /// <summary>
        /// How long until the enemies spawn
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
        /// A list of enemies
        /// </summary>
        public List<Enemy> CurrentEnemies = new List<Enemy>();

        /// <summary>
        /// Should enemies spawn
        /// </summary>
        private bool _shouldSpawn;

        /// <summary>
        /// Called when the game phase changes
        /// </summary>
        /// <param name="newPhase">The new  phase</param>
        public void OnGamePhaseChange(GamePhases newPhase)
        {
            var isFight = newPhase == GamePhases.Fight;
            if (!isFight)
            {
                for (int i = this.CurrentEnemies.Count - 1; i >= 0; i--)
                {
                    var targetEnemy = this.CurrentEnemies[i];
                    if (targetEnemy != null)
                    {
                        Destroy(targetEnemy.gameObject);
                    }
                }
            }

            this._shouldSpawn = isFight;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            SpawnManager.CurrntInstance = this;

            var pathPrefab = PrefabManager.CurrentInstance.SpawnPath;
            // Visualize each path
            foreach (var path in this.Paths)
            {
                var firstPath = Instantiate(pathPrefab);
                firstPath.Attach(path.SpawnPos, path.Nodes[0]); 
                for (int i = 1; i < path.Nodes.Count; i++)
                {
                    var prevNode = path.Nodes[i - 1];
                    var curNode = path.Nodes[i];
                    var newPath = Instantiate(pathPrefab);
                    newPath.Attach(prevNode, curNode);
                }
            }
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            if (this._shouldSpawn)
            {
                var timePassed = Time.deltaTime;

                var prefabManager = PrefabManager.CurrentInstance;
                var gameController = GameController.CurrentInstance;

                // Update each path
                foreach (var path in this.Paths)
                {
                    var timeTillEnd = gameController.TotalDefenseDuration - gameController.TimeSinceFightStart;
                    if (path.StopSpawnBeforeEnd >= timeTillEnd)
                    {
                        continue;
                    }

                    // Update the regular spawns

                    foreach (var regularSpawn in path.RegularSpawns)
                    {
                        regularSpawn.TimeUntilSpawn -= Time.deltaTime;
                        if (regularSpawn.TimeUntilSpawn <= 0)
                        {
                            regularSpawn.TimeUntilSpawn = regularSpawn.SpawnInterval;
                            var newEnemyPrefab = prefabManager.GetEnemyPrefab(regularSpawn.Enemy);
                            if (newEnemyPrefab == null)
                            {
                                Debug.LogError("Prefab for enemy not found: " + regularSpawn.Enemy);
                            }
                            else
                            {
                                var newEnemy = Instantiate(newEnemyPrefab);
                                this.CurrentEnemies.Add(newEnemy);
                                newEnemy.transform.position = path.SpawnPos;
                                newEnemy.Path = path.Nodes;
                            }
                        }
                    }
                }
            }
        }
    }
}
