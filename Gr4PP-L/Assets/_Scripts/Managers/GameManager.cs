using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers {
    public class GameManager : Manager
    {
        public Vector2 DirectionalInput => inputManager.DirectionalInput;

        public static GameManager Instance;
        public DialogueManager dialogueManager;
        public InputManager inputManager;
        public PlayerManager playerManager;
        
        private static Camera s_mainCamera = null;

        void Awake() 
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            dialogueManager = GetComponentInChildren<DialogueManager>();
            inputManager = GetComponentInChildren<InputManager>();
            playerManager = GetComponentInChildren<PlayerManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Camera MainCamera() {
            if (s_mainCamera == null) s_mainCamera = Camera.main;
            return s_mainCamera;
        }
    }   
}