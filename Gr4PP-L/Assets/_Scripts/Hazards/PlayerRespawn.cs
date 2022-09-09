using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private GameMaster _gm;

    private void Start() {
        _gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = _gm.lastCheckpointPos;
    }
}
