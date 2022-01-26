using System;
using UnityEngine;
using _Scripts.Movement.States;
using _Scripts.Utility;
namespace _Scripts.Managers {
    /** Author: Nick Zimanski
    * Version 1/25/22
    */
    public class PlayerManager : MonoBehaviour
    {
        #region Serialized Variables
        [Header("Components")]
        [SerializeField]private _Scripts.Movement.PlayerMovement _playerMovement;
        public Rigidbody2D PlayerRigidbody {get; private set;}
        [Header("Scriptable Objects")]
        [SerializeField]private RunningState _runningState;
        [SerializeField]private GroundedState _groundedState;
        [SerializeField]private SlidingState _slidingState;
        [SerializeField]private GrapplingState _grapplingState;
        [SerializeField]private AirborneState _airborneState;

        [Header("Layer Masks")]
        [SerializeField] private LayerMask _groundLayer;

        [Header("Movement")]
        [SerializeField] private float _groundedAcceleration;
        [SerializeField] private float _groundedDeceleration;
        public float MoveSpeed {get; private set;} //max horizontal speed
        [SerializeField] private float _velPower;
        [Space(10)]
        [SerializeField] private float _frictionAmount;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [Range(0, 1)]
        [SerializeField] private float _jumpCutMultiplier;
        [Space(10)]
        [SerializeField] private float _jumpCoyoteTime;
        [SerializeField] private float _jumpBufferTime;
        [SerializeField] private float _jumpWallJumpSpacing;
        [Space(10)]
        [SerializeField] private float _fallGravityMultiplier;
        [SerializeField] private float _terminalVelocity;
        [Space(10)]

        [Header("Wall Jump")]
        [SerializeField] private float _wallJumpForce;
        [Space(10)]
        [SerializeField] private float _wallJumpCoyoteTime;
        [SerializeField] private float _wallSlideSpeed;
        [SerializeField] private float _wallJumpPreservationTime;
        [Space(10)]

        [Header("Ground Collision")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private Vector2 _wallCheckSize;
        [SerializeField] private Vector2 _wallCheckOffset;
        #endregion
        #region Variables
        private StateMachine<MovementState> _movementSM;
        [NonSerializedAttribute]public bool IsGrounded, IsDead = false, IsJumping = false;
        [NonSerializedAttribute]public float LastGroundJump,
            LastJumpTime,
            LastGroundedTime,
            LastWallJump,
            LastWallTime;
        private float _gravityScale;
        private bool _jumpInputReleased, 
            _leftWall, 
            _rightWall;
        #endregion

        #region User Methods
        private void SetupStateMachine() {
            _movementSM = new StateMachine<MovementState>();

            _runningState.Initialize(this, _movementSM);
            _groundedState.Initialize(this, _movementSM);
            _airborneState.Initialize(this, _movementSM);
            _slidingState.Initialize(this, _movementSM);
            _grapplingState.Initialize(this, _movementSM);
            _movementSM.AddState((int) MovementState.States.Grounded, _groundedState);
            _movementSM.AddState((int) MovementState.States.Airborne, _airborneState);
            _movementSM.AddState((int) MovementState.States.Sliding, _slidingState);
            _movementSM.AddState((int) MovementState.States.Grappling, _grapplingState);
            _movementSM.AddState((int) MovementState.States.Running, _runningState);
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