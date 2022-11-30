using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField]
        private string _interactionAxis;
        [SerializeField]
        private bool _doesPromptPlayer;
        [SerializeField]
        private PromptController _promptController;

        private void PromptPlayer() {
            _promptController.AssignInteractable(_interactionAxis, InteractCallback);
        }

        private void UnpromptPlayer() {
            _promptController.UnassignInteractable();
        }

        public abstract void InteractCallback();

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Player") return;
            if (_doesPromptPlayer) PromptPlayer();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag != "Player") return;
            if (_doesPromptPlayer) UnpromptPlayer();
        }
    }
}
