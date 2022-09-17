using Utility;
using Managers;
using UnityEngine;
namespace Movement {
    /** Author: Nick Zimanski
    * Version 3/21/22
    */
    public abstract class MovementState : State
    {
        [Header("Movement State")]
        [SerializeField]private float _jumpForce;
        #region Variables
        protected MovementStateMachine _sm {get; private set;}
        protected PlayerController _owner {get; private set;}
        protected bool _uncheckedInputBuffer;
        protected float _stateEnterTime;
        protected GrappleHookController _hook;
        protected Rigidbody2D _rb;
        protected GameManager _gameManager;
        /// <summary>
        /// Stores the input data on a frame. Updated automatically every frame before HandleInput()
        /// </summary>

        protected States? _transitionToState;
        public States? Name => null;
        protected bool IsGrounded => GroundedCheck();

        public bool CanGrapple;

        #endregion

        public virtual void Initialize(GameManager game, PlayerController player, MovementStateMachine sm)
        {
            base.Initialize();
            _owner = player;
            _sm = sm;
            _rb = _owner.PlayerRigidbody;
            _uncheckedInputBuffer = false;
            _hook = _owner.GrappleHookCtrl;
            _gameManager = game;
        }
        public override void Enter() {
            base.Enter();
            _uncheckedInputBuffer = true;
            _stateEnterTime = Time.time;
            _transitionToState = null;
        }
        public override void Exit() {
            base.Exit();
        }
        /// <summary>
        /// Checks for inputs and sets appropriate flags. Called during Update.
        /// </summary>
        protected virtual void HandleInput() { }
        ///<summary>
        /// Processes non-physics logic that needs to be checked. Called during Update.
        /// </summary>
        protected virtual void LogicUpdate() {}
        /// <summary>
        /// Processes physics-based logic and moves the player. Called during FixedUpdate.
        /// </summary>
        protected virtual void PhysicsUpdate() {}

        public override void Execute() {
            HandleInput();
            LogicUpdate();
            StateChangeUpdate();
        }
        public override void FixedExecute() {
            PhysicsUpdate();
        }

        protected abstract void CheckInputBuffer();

        protected void StateChangeUpdate() {
            if (_transitionToState != null && _transitionToState != _sm.CurrentState.Name) {
                UnityEngine.Debug.Log("Changing to " + _transitionToState);
                _sm.ChangeState(_sm.GetState((int) _transitionToState));
            }
        }

        protected void GroundedJump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _sm.RemoveBufferedInputsFor("Jump");
            _sm.BufferInput("Grounded Jump", 0.1f);
            _sm.BufferInput("Jumped", 0.15f);
        }

        protected void HandleGrappleInput(Vector2 direction, float force) {
            if (_hook.IsAttached) return;
            if (!_owner.CanGrapple) return;
            

            if (!_hook.IsHeld) {
                _hook.RetractHook();
                return;
            } else {
                 _hook.FireHook(direction, force);
            }
        }
        /// <summary>
        /// Checks if the player is contacting the ground at the grounding box
        /// </summary>
        /// <returns>true if contacting the ground, false otherwise</returns>
        private bool GroundedCheck() {
            return (Physics2D.OverlapBox(_owner.GroundCheckPoint.position - new Vector3(0, 1, 0), _owner.GroundCheckSize, 0, _owner.GroundLayer));
        }

        /// <summary>
        /// Returns the ground collider, used to prevent weird jumps on jump pads
        /// </summary>
        /// <returns>The collider the ground box is touching</returns>
        protected Collider2D GroundCollider() {
            return (Physics2D.OverlapBox(_owner.GroundCheckPoint.position - new Vector3(0, 1, 0), _owner.GroundCheckSize, 0, _owner.GroundLayer));
        }

        /// <summary>
        /// Checks if the player's wall checkboxes are contacting a wall
        /// </summary>
        /// <returns>an int, -1 if the wall is to the left of the player, 1 if it's to the right, and 0 if no contact is made or if both walls are in contact</returns>
        protected int WallCheck() {
            // _wallSide is used instead of directly returning the value. This is done to prevent the player from getting a wall jump when buffering a jump when landing on the ground.
            // previously, the player would occasionally get a wall jump when buffering a jump while landing on the ground because they would clip slightly into the ground, making both
            // wall jump colliders along with the ground collider register. This is fixed by returning 0 when both wall colliders are touching the ground layer.
            // this cannot be fixed by returning 0 if the grounded collider is touching the ground, because sometimes the grounded collider will touch the wall when the player jumps into a wall
            // since they player will slightly clip into the wall.

            int _wallSide = 0;
            Collider2D collision;
            if ((collision = Physics2D.OverlapBox(_owner.GroundCheckPoint.position + new Vector3(-_owner.WallCheckOffset.x, _owner.WallCheckOffset.y, 0), _owner.WallCheckSize, 0, _owner.GroundLayer))
                && !collision.CompareTag("Ice")) {
                _wallSide = -1;
            }
            if ((collision = Physics2D.OverlapBox(_owner.GroundCheckPoint.position + new Vector3(_owner.WallCheckOffset.x, _owner.WallCheckOffset.y, 0), _owner.WallCheckSize, 0, _owner.GroundLayer))
                && !collision.CompareTag("Ice")) {
                if(_wallSide == 0) {
                    _wallSide = 1;
                } else {
                    _wallSide = 0;
                }
            }
            return _wallSide;
        }

        /// <summary>
        /// Checks if the player is moving horizontally faster than the given speed
        /// </summary>
        /// <param name="v">The speed to compare to the player's</param>
        /// <returns>A bool, whether or not the player is moving faster than v</returns>
        protected bool IsPlayerSpeedExceeding(float v)
        {
            if (v == 0 && _rb.velocity.x != 0) return true; 
            return Mathf.Abs(_rb.velocity.x) > Mathf.Abs(v) && Mathf.Sign(v) == Mathf.Sign(_rb.velocity.x);
        }

        new public enum States {
            Grappling = 0,
            Airborne = 1,
            Running = 2,
            Sliding = 3
        };
    }
}
