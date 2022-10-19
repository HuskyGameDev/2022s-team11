using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public class RoomController : MonoBehaviour
    {
        public GameObject virtualCam;
        private GameManager _gm;
        private Vector2 _pos;

        private void Start() {
            _gm = GameManager.Instance;
            _pos = transform.position;
            
            if(GameManager.Instance.levelManager.IsAtCheckpoint(_pos)) {
                virtualCam.SetActive(true);
            }

            if (tag.Equals("Level Origin"))
                _gm.levelManager.RegisterOrigin(gameObject);
        }

        // when the player enters this room, activate this room's camera
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                virtualCam.SetActive(true);
                _gm.levelManager.SetCheckpoint(_pos);
            }
        }

        // when the player leaves this room, deactivate this room's camera
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                virtualCam.SetActive(false);
            }
        }
    }
}