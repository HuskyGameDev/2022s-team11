using _Scripts.Managers;
using _Scripts.Utility;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace _Scripts.Movement.States
{
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    public class MovementStateMachine : StateMachine<MovementState> 
    {
        private Dictionary<string, float> _bufferedInputs = new Dictionary<string, float>();
        /// <summary>
        /// Buffers an input under a given name for a period of time. If the buffer is checked for that input within that period of time, it will act as though the input is still active
        /// </summary>
        /// <param name="inputName">The name of the input, as it appears in GetButton</param>
        /// <param name="timeWindow">The time, in seconds, that the input should be a valid buffer for</param>
        public void BufferInput(string inputName, float timeWindow) {
            if (_bufferedInputs.ContainsKey(inputName)) {
                _bufferedInputs[inputName] = Time.time + timeWindow;
                return;
            }
            _bufferedInputs.Add(inputName, Time.time + timeWindow);
        }

        /// <summary>
        /// Checks the input buffer for a specific input type. If a valid buffered input is found, this will return true.
        /// </summary>
        /// <param name="inputName">The name of the input to check for</param>
        /// <returns>Whether or not a valid buffered input exists</returns>
        public bool CheckBufferedInputsFor(string inputName) {
            if (!_bufferedInputs.ContainsKey(inputName)) return false;
            if (_bufferedInputs[inputName] <= Time.time) return false;
            return true;
        }

        /// <summary>
        /// Checks the input buffer for a specific input type. If a valid buffered input is found, this will return true.
        /// </summary>
        /// <param name="inputName">The name of the input to check for</param>
        /// <param name="consume">Whether or not the buffer should remove the input</param>
        /// <returns>Whether or not a valid buffered input exists</returns>
        public bool CheckBufferedInputsFor(string inputName, bool consume) {
            bool a = CheckBufferedInputsFor(inputName);
            if (consume) RemoveBufferedInputsFor(inputName);
            return a;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputName"></param>
        /// <returns></returns>
        public bool RemoveBufferedInputsFor(string inputName) {
            if (!_bufferedInputs.ContainsKey(inputName)) return false;
            else {
                _bufferedInputs.Remove(inputName);
                return true;
            }
        }

        public void Initialize(GameManager game, MovementState startingState) {
            for (int i = 0; i < _validStates.Count; i++) {
                _validStates.ElementAt(i).Value.Initialize(game, this);
            }
            base.Initialize(startingState);
        }
    }
}