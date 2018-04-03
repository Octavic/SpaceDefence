//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ConstantHitbox.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IConstantHitbox
    {
        void OnHitEnemy(Enemy hitEnemy);
    }
}
