//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SpawnPathIndicator.cs">
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
    /// A small dot that runs down a set path
    /// </summary>
    public class SpawnPathIndicator: MonoBehaviour
    {
        /// <summary>
        /// The speed of the dot
        /// </summary>
        public float Speed;

        /// <summary>
        /// Target path
        /// </summary>
        public Path TargetPath { get; set; }

        private int _curGoalIndex;

        /// <summary>
        /// Starts running
        /// </summary>
        public void StartRunningFromBeginning()
        {
            this._curGoalIndex = 0;
            this.transform.position = this.TargetPath.SpawnPos;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        protected void Update()
        {
            var curGoalNode = this.TargetPath.Nodes[this._curGoalIndex];
            var diffToGoal = curGoalNode  - new Vector2(this.transform.position.x, this.transform.position.y);
            var movementThisFrame = this.Speed * Time.deltaTime;

            // Can reach target
            if (diffToGoal.magnitude < movementThisFrame)
            {
                this._curGoalIndex++;
                this.transform.position = curGoalNode;
             
                // if this is the last index, then start over
                if (this._curGoalIndex >= this.TargetPath.Nodes.Count)
                {
                    this.StartRunningFromBeginning();
                    return;
                }
            }
            else
            {
                // Not reached yet
                var movementToExecute = diffToGoal.normalized * movementThisFrame;
                this.transform.position += new Vector3(movementToExecute.x, movementToExecute.y);
            }
        }
    }
}
