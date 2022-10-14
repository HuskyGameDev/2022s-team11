using System.Security.AccessControl;
using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Level;
using static Level.Powerup;

namespace Managers {
    public class PowerupManager : Manager
    {
        [SerializeField]
        private List<Powerup> _powerupsInScene = new List<Powerup>();


        public void RegisterPowerup(Powerup p) {
            if (_powerupsInScene.Contains(p)) return;
            _powerupsInScene.Add(p);
        }

        public void RespawnPowerups() {
            foreach (var p in _powerupsInScene)
            {
                p.Respawn();
            }
        }


        public PowerupManager() {
            base.Initialize();

            
        }

        public override void Destroy()
        {
            
        }
    }
}
