//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConstantHitbox.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Map.Enemies;

    public interface IConstantHitbox
    {
        void OnHitEnemy(Enemy hitEnemy);
    }
}
