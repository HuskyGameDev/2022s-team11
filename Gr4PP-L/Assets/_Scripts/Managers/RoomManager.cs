using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject virtualCam;
    private GameMaster _gm;

    private void Start() {
        _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if(_gm.lastCheckpointPos == new Vector2 (transform.position.x, transform.position.y)) {
            virtualCam.SetActive(true);
        }
    }

    // when the player enters this room, activate this room's camera
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            virtualCam.SetActive(true);
            _gm.lastCheckpointPos = transform.position;
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
