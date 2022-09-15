using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers {
public class InputManager : Manager
    {

        [Header("Input")]
        [SerializeField]private float _horizAxisThreshold;
        [SerializeField]private float _vertAxisThreshold;

        public Vector2 DirectionalInput {get; private set;}

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            DirectionalInput = GetInput();
        }

        private Vector2 GetInput() {
            var h = UnityEngine.Input.GetAxisRaw("Horizontal");
            var v = UnityEngine.Input.GetAxisRaw("Vertical");
            return new Vector2(Mathf.Abs(h) >= _horizAxisThreshold ? h : 0, Mathf.Abs(v) >= _vertAxisThreshold ? v : 0);
        }
    }
}
