
namespace Assets.Scripts.Map
{
    using Grid.States;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// The save data related to one map node
    /// </summary>
    [Serializable]
    public class MapNodeSaveData
    {
        /// <summary>
        /// Creates a deep copy
        /// </summary>
        /// <param name="copySource">Source of copy</param>
        public MapNodeSaveData(MapNodeSaveData copySource = null)
        {
            if (copySource != null)
            {
                this.LevelId = copySource.LevelId;
                this.SavedState = copySource.SavedState;
                this.HighScore = copySource.HighScore;
                this.IsBeat = copySource.IsBeat;
                this.EnergyCost = copySource.EnergyCost;
                this.GeneratingResources = copySource.GeneratingResources.Select(resource => new MapNodeResource(resource)).ToList();
            }
        }

        /// <summary>
        /// ID for the level
        /// </summary>
        public int LevelId;

        /// <summary>
        /// The saved state for this level 
        /// </summary>
        public MapGridState SavedState = null;

        /// <summary>
        /// Best score achieved
        /// </summary>
        public float HighScore = 0;

        /// <summary>
        /// If the level has been beat yet
        /// </summary>
        public bool IsBeat = false;

        /// <summary>
        /// The energy cost for the last run
        /// </summary>
        public float EnergyCost = 0;

        /// <summary>
        /// The final efficiency
        /// </summary>
        public float Efficiency = 0;

        /// <summary>
        /// The resource that's generated from the map node
        /// </summary>
        public List<MapNodeResource> GeneratingResources = new List<MapNodeResource>();
    }
}
