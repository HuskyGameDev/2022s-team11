using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Managers;

/** Author: Nick Zimanski
    *   Version: 10/25/22
    */
[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public Vector2 DirectionalInput => Get<InputManager>().DirectionalInput;

    public static GameManager Instance { get; private set; }

    [SerializeField]
    private ParameterContainer _parameters;
    public ParameterContainer Parameters => _parameters;
    public static event Action updateCallback;
    public static System.Random Random;
    private Movement.PlayerController _player;
    private bool _isPaused = false;
    private bool _firstFrame = true;
    private PlayerInput _playerInput = null;
    /// <summary>
    /// If you're thinking of using this member, don't. It's not for you :). Use InputManager's methods instead
    /// </summary>
    public PlayerInput RawPlayerInput => _playerInput;
    private readonly Dictionary<string, Manager> _services = new Dictionary<string, Manager>();

    public Movement.PlayerController FindPlayer()
    {
        if (_player == null && Get<LevelManager>().IsSceneLoaded) _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Movement.PlayerController>();
        return _player;
    }

    public static Camera MainCamera { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;

        updateCallback = () => { };
        Random = new System.Random();

        Register<DialogueManager>(new DialogueManager());
        Register<InputManager>(new InputManager());
        Register<LevelManager>(new LevelManager());
        Register<InteractiveManager>(new InteractiveManager());
        Register<TimerManager>(new TimerManager());
        Register<AudioManager>(new AudioManager());
        //Initialize();

        // CHANGE TESTING SCENE HERE
        StartCoroutine(Get<LevelManager>().LoadScene("Tutorial"));
    }

    // Start is called before the first frame update
    void Start()
    {
        FindPlayer();

        MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_firstFrame)
        {
            _firstFrame = false;
            updateCallback?.Invoke();
            return;
        }

        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
        }
        if (Get<InputManager>().GetButtonUp("cancel"))
        {
            if (_isPaused) Resume();
            else Pause();
        }
        if (_isPaused) return;

        updateCallback?.Invoke();
    }

    void Pause()
    {
        _parameters.pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }

    void Resume()
    {
        _parameters.pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    /// <summary>
    /// Kills the current player, then eventually respawns them
    /// </summary>
    /// <param name="pc">The player</param>
    public void KillPlayer(Movement.PlayerController pc)
    {
        pc.Respawn();
    }

    public void Initialize()
    {
        foreach (KeyValuePair<string, Manager> entry in _services)
        {
            string key = entry.Key;
            Manager val = entry.Value;

            //val.Destroy();
            val = val.GetNewInstance();
        }

    }

    public T Get<T>() where T : Manager
    {
        string type = typeof(T).Name;
        if (!_services.ContainsKey(type))
        {
            throw new InvalidOperationException();
        }

        return (T)_services[type];
    }

    private void Register<T>(T service) where T : Manager
    {
        string type = typeof(T).Name;
        if (_services.ContainsKey(type))
        {
            Debug.LogError($"Already have type {type} in the registered services!");
            return;
        }

        service.Initialize();
        _services.Add(type, service);
    }

    private void Unregister<T>() where T : Manager
    {
        string type = typeof(T).Name;
        if (!_services.ContainsKey(type))
        {
            Debug.LogError($"Service {type} not found in the registered services!");
            return;
        }

        _services[type].Destroy();
        _services.Remove(type);
    }

    [System.Serializable]
    public struct ParameterContainer
    {
        [Header("Dialogue")]
        public float charsPerSecond;
        [Header("Input")]
        public float horizAxisThreshold;
        public float vertAxisThreshold;
        [Header("Audio")]
        public Audio.Sound[] sounds;
        [Header("UI")]
        public GameObject loadingScreen;
        public GameObject pauseScreen;
    }
}
