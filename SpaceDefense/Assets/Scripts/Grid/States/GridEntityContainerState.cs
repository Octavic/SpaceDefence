//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="GridEntityContainerState.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Grid.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes the state of a grid entity container
    /// </summary>
    [Serializable]
    public class GridEntityContainerState : GridEntityState
    {
        /// <summary>
        /// The entity that's held
        /// </summary>
        public GridEntityState HoldingEntity;
    }
}
