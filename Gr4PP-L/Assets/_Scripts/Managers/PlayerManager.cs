using UnityEngine;
using _Scripts.Movement.States;
using _Scripts.Utility;
namespace _Scripts.Managers {
    /** Author: Nick Zimanski
    * Version 1/25/22
    */
    public class PlayerManager : MonoBehaviour
    {
        #region Variables
        [SerializeField]private BoxCollider2D _groundingBox;
        [SerializeField]private _Scripts.Movement.PlayerMovement _playerMovement;
        [SerializeField]private Rigidbody2D _playerRigidbody;
        private StateMachine<MovementState> _movementSM;
        public bool IsGrounded;
        #endregion

        #region User Methods
        private void SetupStateMachine() {
            _movementSM = new StateMachine<MovementState>();
            _movementSM.AddState((int) MovementState.States.Grounded, new GroundedState(this, _movementSM));
            _movementSM.AddState((int) MovementState.States.Airborne, new AirborneState(this, _movementSM));
            _movementSM.AddState((int) MovementState.States.Sliding, new SlidingState(this, _movementSM));
            _movementSM.AddState((int) MovementState.States.Grappling, new GrapplingState(this, _movementSM));
            _movementSM.AddState((int) MovementState.States.Running, new RunningState(this, _movementSM));
            _movementSM.Initialize(_movementSM.GetState((int) MovementState.States.Grounded));
        }
        #endregion
        
        #region Unity Callbacks
        void Start()
        {
            SetupStateMachine();
        }

        void Update()
        {
            _movementSM.Update();
        }

        void FixedUpdate()
        {
            _movementSM.FixedUpdate();
        }
        #endregion
    }
}