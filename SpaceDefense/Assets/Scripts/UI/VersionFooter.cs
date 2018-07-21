//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="VersionFooter.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// An UI elements that shows the version information
    /// </summary>
    public class VersionFooter :  MonoBehaviour
    {
        /// <summary>
        /// Used for initialization
        /// </summary>
        protected void Start()
        {
            this.GetComponent<Text>().text = Settings.Version.GetReleaseVersion();
        }
    }
}
