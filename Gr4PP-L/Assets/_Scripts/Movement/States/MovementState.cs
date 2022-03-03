using System.Diagnostics;
using _Scripts.Utility;
using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 2/10/22
    */
    public abstract class MovementState : State
    {
        [Header("Movement State")]
        [SerializeField]private float _jumpForce;
        [SerializeField]private float _horizAxisThreshold;
        [SerializeField]private float _vertAxisThreshold;
        #region Variables
        protected MovementStateMachine _sm {get; private set;}
        protected _Scripts.Managers.PlayerManager _owner {get; private set;}
        protected bool _uncheckedInputBuffer;
        protected float _stateEnterTime;
        protected GrappleHookController _hook;
        protected Rigidbody2D _rb;
        /// <summary>
        /// Stores the input data on a frame. Updated automatically every frame before HandleInput()
        /// </summary>
        protected Vector2 _input;

        protected States? _transitionToState;
        public States? Name => null;
        protected bool IsGrounded => GroundedCheck();
        #endregion
        public virtual void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize();
            _owner = player;
            _sm = sm;
            _rb = player.PlayerRigidbody;
            _uncheckedInputBuffer = false;
            _hook = player.GrappleHookCtrl;
        }
        public override void Enter() {
            base.Enter();
            _uncheckedInputBuffer = true;
            _stateEnterTime = Time.time;
            _transitionToState = null;
            _input = GetInput();
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
            _input = GetInput();
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
        }

        protected void HandleGrappleInput(Vector2 direction, float force) {
            if (_hook.IsAttached) return;
            
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
        /// Checks if the player's wall checkboxes are contacting a wall
        /// </summary>
        /// <returns>an int, -1 if the wall is to the left of the player, 1 if it's to the right, and 0 if no contact is made</returns>
        protected int WallCheck() {
            if (Physics2D.OverlapBox(_owner.GroundCheckPoint.position + new Vector3(-_owner.WallCheckOffset.x, _owner.WallCheckOffset.y, 0), _owner.WallCheckSize, 0, _owner.GroundLayer)) return -1;
            if (Physics2D.OverlapBox(_owner.GroundCheckPoint.position + new Vector3(_owner.WallCheckOffset.x, _owner.WallCheckOffset.y, 0), _owner.WallCheckSize, 0, _owner.GroundLayer)) return 1;
            return 0;
        }

        /// <summary>
        /// Checks if the player is moving horizontally faster than the given speed
        /// </summary>
        /// <param name="v">The speed to compare to the player's</param>
        /// <returns>A bool, whether or not the player is moving faster than v</returns>
        protected bool IsPlayerSpeedExceeding(float v)
        {
            if(Mathf.Abs(_rb.velocity.x) > Mathf.Abs(v) && Mathf.Abs(v) > 0.1f && Mathf.Sign(v) == Mathf.Sign(_rb.velocity.x))
            {
                return true;
            } else
            {
                return false;
            }
        }

        protected Vector2 GetInput() {
            var h = Input.GetAxisRaw("Horizontal");
            var v = Input.GetAxisRaw("Vertical");
            return new Vector2(Mathf.Abs(h) >= _horizAxisThreshold ? h : 0, Mathf.Abs(v) >= _vertAxisThreshold ? v : 0);
        }

        new public enum States {
            Grappling = 0,
            Airborne = 1,
            Running = 2,
            Sliding = 3
        };
    }
}
