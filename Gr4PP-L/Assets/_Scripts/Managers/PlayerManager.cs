using System.Data;
using System;
using UnityEngine;
using _Scripts.Movement.States;
using _Scripts.Movement;
using _Scripts.Utility;
namespace _Scripts.Managers {
    /** Author: Nick Zimanski
    * Version 3/21/22
    */
    public class PlayerManager : Manager
    {
        #region Serialized Variables
        [Header("Components")]
        [SerializeField]private Rigidbody2D _playerRigidbody;
        public Rigidbody2D PlayerRigidbody => _playerRigidbody;
        [SerializeField]private GameObject _grappleHook;
        private GrappleHookController _grappleHookController;
        public GrappleHookController GrappleHookCtrl => _grappleHookController;
        private Rigidbody2D _grappleHookRigidbody;
        public Rigidbody2D GrappleHookRigidbody => _grappleHookRigidbody;
        [SerializeField]private RunningState _runningState;
        [SerializeField]private SlidingState _slidingState;
        [SerializeField]private GrapplingState _grapplingState;
        [SerializeField]private AirborneState _airborneState;
        [SerializeField]private LayerMask _groundLayer;
        public LayerMask GroundLayer => _groundLayer;

        [Header("Collision")]
        [SerializeField]
        [Tooltip("")]
        private Vector2 _wallCheckSize;
        public Vector2 WallCheckSize => _wallCheckSize;
        [SerializeField]
        [Tooltip("")]
        private Vector2 _wallCheckOffset;
        public Vector2 WallCheckOffset => _wallCheckOffset;
        [Space(10)]
        [SerializeField]
        [Tooltip("")]
        private Transform _groundCheckPoint;
        public Transform GroundCheckPoint => _groundCheckPoint;
        [SerializeField]
        [Tooltip("")]
        private Vector2 _groundCheckSize;
        public Vector2 GroundCheckSize => _groundCheckSize;
        #endregion

        #region Variables
        private MovementStateMachine _movementSM;
        /// <summary>
        /// Whether or not the player is touching the ground
        /// </summary>
        public bool IsGrounded => CheckIsGrounded();
        /// <summary>
        /// Whether or not the player is dead
        /// </summary>
        private bool _isDead = false;
        public bool IsDead => _isDead;
        [NonSerializedAttribute]public float LastGroundJump,
            LastJumpTime,
            LastGroundedTime,
            LastWallJump,
            LastWallTime,
            CoyoteTimeWindowEndTime;
        private bool _jumpInputReleased, 
            _isGrappleHeld = true;
        public bool IsGrappleHeld => _isGrappleHeld;

        //GameObject GrappleMarker;
        //GrappleMarker.GetComponent<GrappleActiveScript>();
        #endregion

        #region User Methods
        private void SetupStateMachine() {
            _movementSM = new MovementStateMachine();

            _movementSM.AddState((int) MovementState.States.Airborne, _airborneState);
            _movementSM.AddState((int) MovementState.States.Sliding, _slidingState);
            _movementSM.AddState((int) MovementState.States.Grappling, _grapplingState);
            _movementSM.AddState((int) MovementState.States.Running, _runningState);
            
            _movementSM.Initialize(this, _movementSM.GetState((int) MovementState.States.Running));
        }

        private bool CheckIsGrounded() {
            switch(_movementSM.CurrentState.Name) {
                case MovementState.States.Running:
                case MovementState.States.Sliding:
                    return true;
                default:
                    return false;
            }
        }
        /**
        private bool CheckIsGrappleReady() {
            return 0;
        }
        */
        #endregion
        
        #region Unity Callbacks
        void Start()
        {
            _grappleHookController = _grappleHook.GetComponent<GrappleHookController>();
            _grappleHookRigidbody = _grappleHook.GetComponent<Rigidbody2D>();
            SetupStateMachine();
        }

        void Update()
        {
            _movementSM.Update();
            //print(_movementSM.CurrentState.Name + " " + _movementSM.PreviousState);
        }

        void FixedUpdate()
        {
            _movementSM.FixedUpdate();
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint.position - new Vector3(0,1,0), _groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_groundCheckPoint.position + new Vector3(_wallCheckOffset.x, _wallCheckOffset.y, 0), _wallCheckSize);
            Gizmos.DrawWireCube(_groundCheckPoint.position + new Vector3(-_wallCheckOffset.x, _wallCheckOffset.y, 0), _wallCheckSize);
        }
    }
}