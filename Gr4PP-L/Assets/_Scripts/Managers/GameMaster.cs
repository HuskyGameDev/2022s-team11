using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckpointPos;
    private Vector2 initPos;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(instance);
            initPos = lastCheckpointPos;
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            lastCheckpointPos = initPos;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
