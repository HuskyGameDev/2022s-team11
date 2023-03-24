using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level {
    public class RoomController : MonoBehaviour
    {
        public GameObject virtualCam;
        private GameManager _gm;
        private Vector2 _pos;
        [SerializeField] private int _roomNumber;

        private void Start() {
            _gm = GameManager.Instance;
            _pos = transform.position;
            
            if(_gm.Get<Managers.LevelManager>().IsAtCheckpoint(_pos)) {
                virtualCam.SetActive(true);
            }

            if (tag.Equals("Level Origin"))
                _gm.Get<Managers.LevelManager>().RegisterOrigin(gameObject);
        }

        // when the player enters this room, activate this room's camera
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                virtualCam.SetActive(true);
                _gm.Get<Managers.LevelManager>().SetCheckpoint(_pos, _roomNumber);
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