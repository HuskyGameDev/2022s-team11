using _Scripts.Utility;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/25/22
    */
    public abstract class MovementState : State
    {
        protected StateMachine<MovementState> _sm;
        protected _Scripts.Managers.PlayerManager _player;
        public MovementState(_Scripts.Managers.PlayerManager player, StateMachine<MovementState> sm) {
            _player = player;
            _sm = sm;
        }
        /// <summary>
        /// Checks for inputs and sets appropriate flags.
        /// </summary>
        public virtual void HandleInput() {}
        ///<summary>
        /// Processes non-physics logic that needs to be checked. Called every frame.
        /// </summary>
        public virtual void LogicUpdate() {}
        /// <summary>
        /// Processes physics-based logic and moves the player.
        /// </summary>
        public virtual void PhysicsUpdate() {}

        new public enum States {
            Grounded = 0,
            Airborne = 1,
            Running = 2,
            Sliding = 3,
            Grappling = 4
        };
    }
}
