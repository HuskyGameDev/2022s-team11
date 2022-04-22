using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckpointPos;
    public float lastTime;
    private float initTime = 0;
    private Vector2 initPos;

    private void Awake() {
        if (instance == null) {
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
            lastTime = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
