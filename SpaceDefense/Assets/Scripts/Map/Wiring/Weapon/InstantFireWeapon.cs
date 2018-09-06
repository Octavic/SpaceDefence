
namespace Assets.Scripts.Map.Wiring.Weapon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines a weapon that will fire when toggled without any extra mechanics
    /// </summary>
    public abstract class InstantFireWeapon : Weapon
    {
        /// <summary>
        /// The  weapon fire mode
        /// </summary>
        public WeaponFireMode FireMode;

        /// <summary>
        /// Called when the cooldown ends
        /// </summary>
        protected override void OnCooldownEnd()
        {
            if (this.FireMode == WeaponFireMode.Rapid && this.Inputs.Any(input => input.IsOn))
            {
                this.OnFire();
            }

            base.OnCooldownEnd();
        }

        /// <summary>
        /// Called when there was a change in input
        /// </summary>
        public override void OnInputChange()
        {
            var fired = this.Inputs.Any(input => input.IsOn);
            if (fired && !this.InCooldown)
            {
                this.OnFire();
            }
        }
    }
}
