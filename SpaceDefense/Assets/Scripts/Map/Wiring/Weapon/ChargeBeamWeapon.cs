//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="ChargeBeamWeapon.cs">
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

    /// <summary>
    /// Defines a weapon that charges up when input is on, and fires when released
    /// </summary>
    public class ChargeBeamWeapon : Weapon
    {
        /// <summary>
        /// How much charge gain per second charged  (Charge level goes from 0 to 1)
        /// </summary>
        public float ChargeSpeed;

        /// <summary>
        /// How much it costs to charge the weapon per second
        /// </summary>
        public float ChargeCost;

        /// <summary>
        /// The actual beam weapon
        /// </summary>
        public ChargeBeamObject BeamObject;

        /// <summary>
        /// If the weapon is charging up
        /// </summary>
        private bool _isCharging = false;

        /// <summary>
        /// How high up the weapon was charged
        /// </summary>
        private float ChargeLevel
        {
            get
            {
                return this._charageLevel;
            }
            set
            {
                value = Mathf.Max(0, value);
                value = Mathf.Min(1, value);
                this._charageLevel = value;
            }
        }

        /// <summary>
        /// The charge level
        /// </summary>
        private float _charageLevel = 0;

        /// <summary>
        /// Called when the input changes
        /// </summary>
        public override void OnInputChange()
        {
            if (this.Inputs.Any(input => input.IsOn))
            {
                this._isCharging = true;
            }
            else
            {
                this._isCharging = false;
                this.OnFire();
            }
        }

        /// <summary>
        /// Called when the weapon is fired
        /// </summary>
        /// <returns></returns>
        protected override GameObject OnFire()
        {
            this.BeamObject.Activate(this._charageLevel);
            GameController.CurrentInstance.AddCost(this.Cost);
            this._charageLevel = 0;
            return this.BeamObject.gameObject;
        }

        /// <summary>
        /// Called  once per frame
        /// </summary>
        protected override void Update()
        {
            if (this._isCharging)
            {
                this.ChargeLevel += this.ChargeSpeed * Time.deltaTime;
                GameController.CurrentInstance.AddCost(this.ChargeCost * Time.deltaTime);
            }

            base.Update();
        }
    }
}
