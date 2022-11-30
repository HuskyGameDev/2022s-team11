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
            p_isActive = true;
            gameObject.SetActive(true);
        }

        protected void Deactivate() {
            p_isActive = false;
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            Managers.InteractiveManager.OnObjectRespawn -= Respawn;
        }
    }
}