using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Managers
{
    /** Author: Nick Zimanski
    *   Version: 01/30/23
    */
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : Manager
    {
        private PlayerInput _pi = null;
        public InputAction _fireAction;
        public InputAction _resetAction;
        public InputAction _moveAction;
        public InputAction _submitAction;



        /*************************************************/
        private InputActionMap _playerActionMap;
        private InputActionMap _uiActionMap;

        private float _horizAxisThreshold, _vertAxisThreshold;
        private readonly float DIRECTIONAL_KEYBOARD_THRESHOLD = 0.7f;

        public Vector2 DirectionalInput { get; private set; }

        // Update is called once per frame
        public void Update()
        {
            //Initialize input actions
            if (_pi == null)
            {
                _pi = GameManager.Instance.RawPlayerInput;
            }
            DirectionalInput = _pi.actions["move"].ReadValue<Vector2>();

            if (_pi.currentControlScheme == "Keyboard&Mouse")
            {
                Vector2 newVec = new Vector2(0, 0);
                if (Math.Abs(DirectionalInput.x) > DIRECTIONAL_KEYBOARD_THRESHOLD)
                {
                    newVec.x = 1 * Math.Sign(DirectionalInput.x);
                }
                if (Math.Abs(DirectionalInput.y) > DIRECTIONAL_KEYBOARD_THRESHOLD)
                {
                    newVec.y = 1 * Math.Sign(DirectionalInput.y);
                }
                DirectionalInput = newVec;
            }
        }

        /// <summary>
        /// Returns a value between -1 and 1 for the given axis, with no smoothing applied
        /// </summary>
        /// <param name="axisName">The unity name for this axis</param>
        /// <returns>A value between -1 and 1 representing this input axis</returns>
        public T GetAxis<T>(string axisName) where T : struct
        {
            return _pi.actions[axisName].ReadValue<T>();
        }

        /// <summary>
        /// Returns true while the button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button is depressed</returns>
        public bool GetButton(string inputName)
        {
            return _pi.actions[inputName].GetButton();
        }

        /// <summary>
        /// Returns true on the frame that a button is released
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was released on this frame</returns>
        public bool GetButtonUp(string inputName)
        {
            return _pi.actions[inputName].GetButtonUp();
        }

        /// <summary>
        /// Returns true on the frame that a button is depressed
        /// </summary>
        /// <param name="inputName">The unity name for the button</param>
        /// <returns>true if the button was first depressed this frame</returns>
        public bool GetButtonDown(string inputName)
        {
            return _pi.actions[inputName].GetButtonDown();
        }

        public void LockInput(string inputName)
        {
            _pi.actions[inputName].Disable();
        }
        public void LockInputType(string inputTypeName)
        {
            _pi.actions.FindActionMap(inputTypeName).Disable();
        }

        public void UnlockInput(string inputName)
        {
            _pi.actions[inputName].Enable();
        }
        public void UnlockInputType(string inputTypeName)
        {
            _pi.actions.FindActionMap(inputTypeName).Enable();
        }

        public void StartRebind(string axisName)
        {
            _pi.actions[axisName].PerformInteractiveRebinding()
                .WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete())
                .Start();
        }

        public String GetControlType()
        {
            return _pi.currentControlScheme;
        }

        private void RebindComplete()
        {

        }
        /*
        *
        *   Other Classes
        *
        */
        public new void Initialize()
        {
            base.Initialize();

            _playerActionMap = new InputActionMap();
            _uiActionMap = new InputActionMap();


            _horizAxisThreshold = GameManager.Instance.Parameters.horizAxisThreshold;
            _vertAxisThreshold = GameManager.Instance.Parameters.vertAxisThreshold;

            GameManager.updateCallback += Update;
        }

        public InputManager()
        {
            Initialize();
        }

        public override Manager GetNewInstance()
        {
            return new InputManager();
        }

        public override void Destroy()
        {
            GameManager.updateCallback -= Update;
        }
    }
}
