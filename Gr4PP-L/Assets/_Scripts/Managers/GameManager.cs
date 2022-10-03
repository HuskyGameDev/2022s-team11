using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

public class GameManager : Manager
{
    public Vector2 DirectionalInput => inputManager.DirectionalInput;

    public static GameManager Instance {get; private set;}
    public DialogueManager dialogueManager;
    public InputManager inputManager;
    public LevelManager levelManager;
    public TimerManager timerManager;
    public PowerupManager powerupManager;
    public AudioManager audioManager;

    private Movement.PlayerController _player;
    public Movement.PlayerController GetPlayer() {
        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement.PlayerController>();
        return _player;
    }

    public static Camera MainCamera {get; private set;}

    void Awake() 
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;

        dialogueManager = GetComponentInChildren<DialogueManager>();
        inputManager = GetComponentInChildren<InputManager>();
        levelManager = GetComponentInChildren<LevelManager>();
        timerManager = GetComponentInChildren<TimerManager>();
        powerupManager = GetComponentInChildren<PowerupManager>();
        audioManager = GetComponentInChildren<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement.PlayerController>();
        
        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        #region Scene Resetting
        if (inputManager.GetButton("Level Reset")) {
            GetPlayer().ResetPosition();
            LevelManager.ResetScene();
        }
        #endregion
    }

    /// <summary>
    /// Kills the current player, then eventually respawns them
    /// </summary>
    /// <param name="pc">The player</param>
    public void KillPlayer(Movement.PlayerController pc) {
        powerupManager.RespawnPowerups();
        pc.Respawn();
    }

    public override void OnSceneReset() {
        dialogueManager.OnSceneReset();
        inputManager.OnSceneReset();
        levelManager.OnSceneReset();
        timerManager.OnSceneReset();
        powerupManager .OnSceneReset();
        audioManager.OnSceneReset();

        Start();
    }
}   