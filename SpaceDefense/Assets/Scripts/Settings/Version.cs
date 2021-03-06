﻿//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="Version.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Settings
{
    /// <summary>
    /// All version related information
    /// </summary>
    public static class Version
    {
        public const int MajorVersion = 1;
        public const int MinorVersion = 2;
        public const int PointRelease = 0;
        public const string GitHash = "";

        public static string GetReleaseVersion()
        {
            return "v" + MajorVersion + "." + MinorVersion + "." + PointRelease + "-" + GitHash;
        }
    }
}
