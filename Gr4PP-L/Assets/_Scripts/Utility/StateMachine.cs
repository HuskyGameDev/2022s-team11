using System.Collections.Generic;
namespace _Scripts.Utility {
    /** Author: Nick Zimanski
    * Version 1/25/22
    */
    public class StateMachine<T> where T : State
    {
        protected Dictionary<int, T> _validStates;
        protected T _currentState;
        public T CurrentState => _currentState;
        public StateMachine(T state) {
            Initialize(state);
        }
        public StateMachine() {}

        public void Initialize(T newState) {
            _currentState = newState;

            if (_currentState != null) {
                _currentState.Enter();
            }
        }

        public void ChangeState(T newState) {
            if (_currentState != null) {
                _currentState.Exit();
            }

            _currentState = newState;

            if (_currentState != null) {
                _currentState.Enter();
            }
        }

        public T GetState(int key) {
            return _validStates[key];
        }

        public void AddState(int key, T state) {
            _validStates.Add(key, state);
        }

        public void Update() {
            if (_currentState != null) {
                _currentState.Execute();
            }
        }

        public void FixedUpdate() {
            if (_currentState != null) {
                _currentState.FixedExecute();
            }
        }
    }
}
