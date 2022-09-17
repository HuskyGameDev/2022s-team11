using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;

public class GameManager : Manager
{
    public Vector2 DirectionalInput => inputManager.DirectionalInput;

    public static GameManager Instance {get; private set;}
    public DialogueManager dialogueManager;
    public InputManager inputManager;

    [SerializeField] private Vector2 _lastCheckpointPos;
    [SerializeField] private float _lastTime;
    private float _initTime = 0;
    private Vector2 _initPos;

    public static Camera MainCamera {get; private set;}

    void Awake() 
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GetComponentInChildren<DialogueManager>();
        inputManager = GetComponentInChildren<InputManager>();
        
        MainCamera = Camera.main;

        _initPos = _lastCheckpointPos;
    }

    // Update is called once per frame
    void Update()
    {
        #region Player Resetting
        if (inputManager.GetButton("Player Reset")) {
            _lastCheckpointPos = _initPos;
            _lastTime = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion
    }
}   