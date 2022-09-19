using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public abstract class Powerup : MonoBehaviour
    {
        private SpriteRenderer _sr;
        private bool _active;

        private void Start() {
            _sr = gameObject.GetComponent<SpriteRenderer>();
            _active = true;
            GameManager.Instance.powerupManager.RegisterPowerup(this);
        }

        public GameObject GetGameObject() {
            return gameObject;
        }

        public void Respawn() {
            if (_active) return;

            _sr.enabled = true;
            _active = true;
        }

        protected void Pickup() {
            if (!_active) return;

            _sr.enabled = false;
            _active = false;
        }
    }
}
