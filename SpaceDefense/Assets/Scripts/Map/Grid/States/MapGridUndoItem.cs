//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="MapGridUndoItem.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Grid.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines a map grid state that includes the diff in materials
    /// </summary>
    [Serializable]
    public class MapGridUndoItem
    {
        public MapGridState SaveState;
        public List<ResourceCost> DiffFromPrev;

        public MapGridUndoItem(MapGridState saveState, List<ResourceCost> costs = null)
        {
            this.SaveState = saveState;
            if(costs != null)
            {
                this.DiffFromPrev = costs.Select(this.ReserveCost).ToList();
            }
            else
            {
                this.DiffFromPrev = new List<ResourceCost>();
            }
        }

        private ResourceCost ReserveCost(ResourceCost cost)
        {
            var result = new ResourceCost();
            result.Resource = cost.Resource;
            result.Amount = -cost.Amount;
            return result;
        }
    }
}
