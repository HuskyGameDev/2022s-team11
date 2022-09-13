using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers {
    public class GameManager : Manager
    {

        [Header("Input")]
        [SerializeField]private float _horizAxisThreshold;
        [SerializeField]private float _vertAxisThreshold;
        private Vector2 _input;
        public Vector2 Input => _input;

        public static GameManager gameManager;
        
        private static Camera s_mainCamera = null;

        void Awake() 
        {
            gameManager = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _input = GetInput();
        }

        public static Camera MainCamera() {
            if (s_mainCamera == null) s_mainCamera = Camera.main;
            return s_mainCamera;
        }

        private Vector2 GetInput() {
            var h = UnityEngine.Input.GetAxisRaw("Horizontal");
            var v = UnityEngine.Input.GetAxisRaw("Vertical");
            return new Vector2(Mathf.Abs(h) >= _horizAxisThreshold ? h : 0, Mathf.Abs(v) >= _vertAxisThreshold ? v : 0);
        }
    }   
}