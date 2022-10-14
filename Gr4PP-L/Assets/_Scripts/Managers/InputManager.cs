using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class InputManager : Manager
    {

        private InputData[] _inputAxes;
        private Dictionary<string, ControlType[]> _inputTags;
        private Dictionary<ControlType, bool> _lockedInputs;

        private bool _inputLocked = false;
        private float _horizAxisThreshold, _vertAxisThreshold;

        public Vector2 DirectionalInput {get; private set;}

        // Update is called once per frame
        public void Update()
        {
            DirectionalInput = GetInput();
        }

        /// <summary>
        /// Returns a value between -1 and 1 for the given axis, with no smoothing applied
        /// </summary>
        /// <param name="axisName">The unity name for this axis</param>
        /// <returns>A value between -1 and 1 representing this input axis</returns>
        public float GetAxisRaw(string axisName) {
            if (IsLocked(axisName)) return 0;
            return Input.GetAxisRaw(axisName);
        }

        /// <summary>
        /// Returns true while the button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button is depressed</returns>
        public bool GetButton(string inputName) {
            if (IsLocked(inputName)) return false;
            return Input.GetButton(inputName);
        }

        /// <summary>
        /// Returns true on the frame that a button is released
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was released on this frame</returns>
        public bool GetButtonUp(string inputName) {
            if (IsLocked(inputName)) return false;
            return Input.GetButtonUp(inputName);
        }
        
        /// <summary>
        /// Returns true on the frame that a button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was first depressed this frame</returns>
        public bool GetButtonDown(string inputName) {
            if (IsLocked(inputName)) return false;
            return Input.GetButtonDown(inputName);
        }

        private Vector2 GetInput() {
            var h = IsLocked("Horizontal") ? 0 : UnityEngine.Input.GetAxisRaw("Horizontal");
            var v = IsLocked("Vertical") ? 0 : UnityEngine.Input.GetAxisRaw("Vertical");
            return new Vector2(Mathf.Abs(h) >= _horizAxisThreshold ? h : 0, Mathf.Abs(v) >= _vertAxisThreshold ? v : 0);
        }

        private void RegisterInputs() {
            foreach (var input in _inputAxes)
            {
                _inputTags.Add(input.name, input.tags);
            }

            foreach (var item in Enum.GetNames(typeof(ControlType)))
            {
                _lockedInputs.Add(((ControlType) Enum.Parse(typeof(ControlType), item)), false);
            }
        }

        public void LockType(ControlType type) {
            _lockedInputs[type] = true;
            Debug.Log("Locking "+ type + " inputs");
        }

        public void UnlockType(ControlType type) {
            _lockedInputs[type] = false;
        }

        
        public bool IsLocked(string inputName) {
            foreach (var item in _inputTags[inputName])
            {
                if (_lockedInputs[item]) return true;
            }
            return false;
        }

        /*
        *
        *   Other Classes
        *
        */

        [System.Serializable]
        public class InputData {
            public string name;
            public ControlType[] tags;
        }

        public enum ControlType {
            MOVEMENT,
            INTERACTION,
            SYSTEM
        }
        public InputManager() {
            base.Initialize();
            

            _horizAxisThreshold = GameManager.Instance.Parameters.horizAxisThreshold;
            _vertAxisThreshold = GameManager.Instance.Parameters.vertAxisThreshold;
            _inputAxes = GameManager.Instance.Parameters.inputAxes;

            _lockedInputs = new Dictionary<ControlType, bool>();
            _inputTags = new Dictionary<string, ControlType[]>();
            RegisterInputs();

            GameManager.updateCallback += Update;
        }

        public override void Destroy()
        {
            GameManager.updateCallback -= Update;
        }
    }
}
