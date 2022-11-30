using System.Data;
using System;
using UnityEngine;
using Movement;
using Utility;

namespace Movement
{
    /** Author: Nick Zimanski
    * Version 3/21/22
    */
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Variables
        [Header("Components")]
        [SerializeField] private Rigidbody2D _playerRigidbody;
        public Rigidbody2D PlayerRigidbody => _playerRigidbody;

        [SerializeField] private GameObject _grappleHook;

        public GrappleHookController GrappleHookCtrl { get; private set; }
        public Rigidbody2D GrappleHookRigidbody { get; private set; }

        [Header("States")]
        [SerializeField] private RunningState _runningState;
        [SerializeField] private SlidingState _slidingState;
        [SerializeField] private GrapplingState _grapplingState;
        [SerializeField] private AirborneState _airborneState;
        [SerializeField] private LayerMask _groundLayer;
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
        [NonSerializedAttribute]
        public float LastGroundJump,
            LastJumpTime,
            LastGroundedTime,
            LastWallJump,
            LastWallTime,
            CoyoteTimeWindowEndTime;
        private bool _jumpInputReleased,
            _isGrappleHeld = true;
        private Vector2 _initialPosition;

        public bool IsGrappleHeld => _isGrappleHeld;

        public bool CanGrapple;
        private GameManager _gm;
        #endregion

        #region User Methods
        private void SetupStateMachine()
        {
            _movementSM = new MovementStateMachine();

            _movementSM.AddState((int)MovementState.States.Airborne, _airborneState);
            _movementSM.AddState((int)MovementState.States.Sliding, _slidingState);
            _movementSM.AddState((int)MovementState.States.Grappling, _grapplingState);
            _movementSM.AddState((int)MovementState.States.Running, _runningState);

            _movementSM.Initialize(_gm, this, _movementSM.GetState((int)MovementState.States.Running));
        }

        private bool CheckIsGrounded()
        {
            switch (_movementSM.CurrentState.Name)
            {
                case MovementState.States.Running:
                case MovementState.States.Sliding:
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Sets the player to their origin position
        /// </summary>
        public void ResetPositionToOrigin()
        {
            SetPosition(_initialPosition);
        }

        /// <summary>
        /// Sets the player to their last checkpoint position
        /// </summary>
        public void ResetPositionToCheckpoint()
        {
            SetPosition(_gm.Get<Managers.LevelManager>().GetLastCheckpoint());
        }

        /// <summary>
        /// Sets the player to any position in the world
        /// </summary>
        /// <param name="pos">the position to set to</param>
        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
            _playerRigidbody.velocity = Vector2.zero;
        }

        /// <summary>
        /// Resets the player's position to their last checkpoint
        /// </summary>
        public void Respawn()
        {
            _gm.Get<Managers.LevelManager>().ResetLevel();
        }

        private void Die()
        {
            GrappleHookCtrl.RetractHook();
            _playerRigidbody.velocity = Vector2.zero;

            _gm.Get<Managers.AudioManager>().PlayVariantPitch("Death " + GameManager.Random.Next(2));
        }
        #endregion

        #region Unity Callbacks
        void Start()
        {
            _gm = GameManager.Instance;

            GrappleHookCtrl = _grappleHook.GetComponent<GrappleHookController>();
            GrappleHookRigidbody = _grappleHook.GetComponent<Rigidbody2D>();
            SetupStateMachine();

            _initialPosition = transform.position;

            _gm.Get<Managers.LevelManager>().RegisterOrigin(_initialPosition);
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

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Hazard"))
            {
                //We hit a hazard, time to die
                Die();
                _gm.KillPlayer(this);

            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_groundCheckPoint.position - new Vector3(0, 1, 0), _groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_groundCheckPoint.position + new Vector3(_wallCheckOffset.x, _wallCheckOffset.y, 0), _wallCheckSize);
            Gizmos.DrawWireCube(_groundCheckPoint.position + new Vector3(-_wallCheckOffset.x, _wallCheckOffset.y, 0), _wallCheckSize);
        }
    }
}