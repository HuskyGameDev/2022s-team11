using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class InputManager : Manager
    {

        [Header("Input")]
        [SerializeField]private float _horizAxisThreshold;
        [SerializeField]private float _vertAxisThreshold;

        private bool _inputLocked = false;

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

        /// <summary>
        /// Returns a value between -1 and 1 for the given axis, with no smoothing applied
        /// </summary>
        /// <param name="axisName">The unity name for this axis</param>
        /// <returns>A value between -1 and 1 representing this input axis</returns>
        public float GetAxisRaw(string axisName) {
            if (_inputLocked) return 0;
            return Input.GetAxisRaw(axisName);
        }

        /// <summary>
        /// Returns true while the button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button is depressed</returns>
        public bool GetButton(string inputName) {
            if (_inputLocked) return false;
            return Input.GetButton(inputName);
        }

        /// <summary>
        /// Returns true on the frame that a button is released
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was released on this frame</returns>
        public bool GetButtonUp(string inputName) {
            if (_inputLocked) return false;
            return Input.GetButtonUp(inputName);
        }
        
        /// <summary>
        /// Returns true on the fram that a button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was first depressed this frame</returns>
        public bool GetButtonDown(string inputName) {
            if (_inputLocked) return false;
            return Input.GetButtonDown(inputName);
        }

        private Vector2 GetInput() {
            var h = UnityEngine.Input.GetAxisRaw("Horizontal");
            var v = UnityEngine.Input.GetAxisRaw("Vertical");
            return new Vector2(Mathf.Abs(h) >= _horizAxisThreshold ? h : 0, Mathf.Abs(v) >= _vertAxisThreshold ? v : 0);
        }
    }
}
