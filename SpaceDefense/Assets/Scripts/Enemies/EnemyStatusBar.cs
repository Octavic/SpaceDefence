//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="EnemyStatusBar.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Enemies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;

    /// <summary>
    /// Describes the status bar that shows all of the icon for active statuses on an enemy
    /// </summary>
    public class EnemyStatusBar : MonoBehaviour
    {
        /// <summary>
        /// A list of icons 
        /// </summary>
        public List<EnemyStatusIcon> Icons = new List<EnemyStatusIcon>();

        /// <summary>
        /// Rearrange all of the icons
        /// </summary>
        private void RearrangeAllIcons()
        {
            for (int i = 0; i < this.Icons.Count; i++)
            {
                this.Icons[i].transform.localPosition = new Vector3(i * GeneralSettings.IconSize, 0);
            }
        }

        /// <summary>
        /// Called when the given effect expires
        /// </summary>
        public void RemoveEffect(EffectEnum targetEffect)
        {
            var index = this.Icons.FindIndex(icon => icon.TargetEffect == targetEffect);
            if (index >= 0)
            {
                this.Icons.RemoveAt(index);
                this.RearrangeAllIcons();
            }
        }

        public void AddEffect(EffectEnum targetEffect)
        {
            var index = this.Icons.FindIndex(icon => icon.TargetEffect == targetEffect);
            if (index < 0)
            {
                var newIcon = Instantiate(PrefabManager.CurrentInstance.GetEnemyStatusIcon(targetEffect), this.transform);
                newIcon.transform.localPosition = new Vector3(this.Icons.Count * GeneralSettings.IconSize, 0);
                this.Icons.Add(newIcon);
            }
        }
    }
}
