//  --------------------------------------------------------------------------------------------------------------------
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
        public const int MajorVersion = 0;
        public const int MinorVersion = 5;
        public const int PointRelease = 0;
        public const string GitHash = "";

        public static string GetReleaseVersion()
        {
            return "v" + MajorVersion + "." + MinorVersion + "." + PointRelease + "-" + GitHash;
        }
    }
}
