using UnityEngine;

namespace Level {
    /** Author: Nick Zimanski
    * Version: 9/19/22
    */
    public abstract class Powerup : Respawnable
    {

        protected new void Start() {
            base.Start();
            p_isActive = true;
        }

        protected void Pickup() {
            if (!p_isActive) return;

            Deactivate();
        }
    }
}
