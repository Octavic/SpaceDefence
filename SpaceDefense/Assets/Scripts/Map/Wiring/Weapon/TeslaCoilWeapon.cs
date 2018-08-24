//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="TeslaCoilWeapon.cs">
//    Copyright (c) Yifei Xu .  All rights reserved.
//  </copyright>
//  --------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Map.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Settings;

    /// <summary>
    /// Defines a Tesla coil weapon that goes zappy when other coils are nearby
    /// </summary>
    public class TeslaCoilWeapon : Weapon
    {
        /// <summary>
        /// Prefab for the tesla bolt
        /// </summary>
        public TeslaCoilBolt BoltPrefab;

        /// <summary>
        /// A list of all tesla coil weapons
        /// </summary>
        private static HashSet<TeslaCoilWeapon> _coils = new HashSet<TeslaCoilWeapon>();

        /// <summary>
        /// A collction of targetWeapon => connected beam
        /// </summary>
        private Dictionary<TeslaCoilWeapon, TeslaCoilBolt> _attachedBeams = new Dictionary<TeslaCoilWeapon, TeslaCoilBolt>();

        /// <summary>
        /// If the coil is on
        /// </summary>
        private bool _isOn = false;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            this._isOn = this.Inputs.Any(input => input.IsOn);
        }

        /// <summary>
        /// Called when the item is moved
        /// </summary>
        public override void OnMove()
        {
            this.DrawBeams(this);
            base.OnMove();
        }

        /// <summary>
        /// Draws all of the beams
        /// </summary>
        private void DrawBeams(TeslaCoilWeapon source)
        {
            // Remove old beams
            var attachedCoils = new List<TeslaCoilWeapon>(this._attachedBeams.Keys);
            foreach (var attached in attachedCoils)
            {
                this._attachedBeams.Remove(attached);
            }

            // Draw new beams
            var curPos = this.transform.position;
            var coilsInRange = _coils.Where(
                coil =>
                    coil._isOn &&
                    coil != this &&
                    coil != source &&
                    (coil.transform.position - curPos).magnitude < WeaponSettings.TeslaCoilRange
                );
            foreach (var coil in coilsInRange)
            {
                var newBeam = Instantiate(this.BoltPrefab).GetComponent<TeslaCoilBolt>();
                newBeam.Attach(curPos, coil.transform.position);
                this._attachedBeams[coil] = newBeam;
            }

            // Notify connected weapons to draw beams 
            foreach (var attached in attachedCoils)
            {
                attached.DrawBeams(this);
            }
        }

        /// <summary>
        /// Called when the coil is fired
        /// </summary>
        /// <returns>The fired projectile</returns>
        protected override GameObject OnFire()
        {
            // Doesn't do anything, all actions are handled in update
            return null;
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        protected override void Start()
        {
            _coils.Add(this);
            base.Start();
        }

        /// <summary>
        /// Called when the gameobject is destroyed
        /// </summary>
        protected void OnDestroy()
        {
            _coils.Remove(this);
        }
    }
}
