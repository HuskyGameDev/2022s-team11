using UnityEngine;
namespace _Scripts.Utility {
    /** Author: Nick Zimanski
    * Version 1/25/22
    */    
    public class State : ScriptableObject
    {
        public enum States {}
        /**protected StateMachine<State> _sm;
        public State(StateMachine<State> sm) {
            _sm = sm;
        }*/

        /// <summary>
        /// Called when changing to this state.
        /// </summary>
        public virtual void Enter() {}
        /// <summary>
        /// Runs all logic related tasks. Called every frame.
        /// </summary>
        public virtual void Execute() {}
        /// <summary>
        /// Called every fixed period of time. Runs all physics-related tasks.
        /// </summary>
        public virtual void FixedExecute() {}
        /// <summary>
        /// Called when changing away from this state.
        /// </summary>
        public virtual void Exit() {}
        /// <summary>
        /// Called to initialize any data in this object
        /// </summary>
        public virtual void Initialize() {}
    }
}