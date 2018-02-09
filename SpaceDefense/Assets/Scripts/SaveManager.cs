//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="SaveManager.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using UnityEngine;
    using Grid;
    using Grid.States;

    /// <summary>
    /// Describes a save manager
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current instance of the <see cref="SaveManager"/> class
        /// </summary>
        public static SaveManager CurrentInstance
        {
            get
            {
                if (_currentInstance == null)
                {
                    _currentInstance = GameObject.FindGameObjectWithTag(Tags.SaveManager).GetComponent<SaveManager>();
                }

                return _currentInstance;
            }
        }
        private static SaveManager _currentInstance;

        /// <summary>
        /// The save path
        /// </summary>
        private string _savePath;

        /// <summary>
        /// Save the state into a file
        /// </summary>
        /// <param name="state">Target state to save</param>
        public void SaveData(MapGridState state)
        {
            var formatter = new BinaryFormatter();
            var targetFile = File.Open(this._savePath, FileMode.Create);

            formatter.Serialize(targetFile, state);
            targetFile.Close();
        }

        /// <summary>
        /// Loads the saved state from data
        /// </summary>
        /// <returns>The save state, null if inone</returns>
        public MapGridState LoadState()
        {
            try
            {
                var formatter = new BinaryFormatter();
                var targetFile = File.Open(this._savePath, FileMode.Open);

                var result = (MapGridState)formatter.Deserialize(targetFile);
                targetFile.Close();
                return result;
            }
            catch
            {
                Debug.Log("No save data");
                return null;
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this._savePath = Application.persistentDataPath + "/save.dat";
        }
    }
}
