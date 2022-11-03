using UnityEngine;

namespace Level
{
    public abstract class Respawnable : MonoBehaviour
    {
        protected bool p_isActive;

        protected void Start()
        {
            Managers.InteractiveManager.OnObjectRespawn += Respawn;
        }

        public GameObject GetGameObject() {
            return gameObject;
        }

        public void Respawn() {
            Activate();
        }

        protected void Deactivate() {
            p_isActive = false;
            gameObject.SetActive(false);
        }

        protected void Activate() {
            p_isActive = true;
            gameObject.SetActive(true);
        }
    }
}