﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ITransformer.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Transformers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Describes an item that takes 
    /// </summary>
    public interface ITransformer : Emitters.IEmitter, IReceiver
    {
    }
}
