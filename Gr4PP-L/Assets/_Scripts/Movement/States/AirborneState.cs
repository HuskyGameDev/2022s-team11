using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 2/10/22
    */
    [CreateAssetMenu(fileName = "AirborneStateData", menuName = "ScriptableObjects/MovementStates/AirborneStateScriptableObject")]
    public class AirborneState : MovementState
    {
        [Header("Jump")]
        [SerializeField]
        [Tooltip("How long, in seconds, the player has after leaving a platform to jump")]
        private float _jumpCoyoteTime;
        [SerializeField]
        [Tooltip("How long, in seconds, a jump input will be valid for if entered before touching the ground")]
        private float _jumpBufferTime;
        [SerializeField]
        [Tooltip("How long, in seconds, a player must wait after jumping before they can wall jump")]
        private float _groundJumpToWallJumpDelay;
        [SerializeField]
        [Tooltip("How hard to push the player back down when they end their jump")]
        [Range(0,1)]
        private float _jumpCutMultiplier;
        [SerializeField]
        [Tooltip("How much force to apply to the player when jumping them off a wall.")]
        private float _wallJumpForce;

        [Header("Velocity")]
        [SerializeField]
        [Tooltip("How quickly to ramp the player's gravity when falling")]
        private float _fallGravityMultiplier;
        [SerializeField]
        [Tooltip("The maximum vertical speed the player can attain")]
        private float _terminalVelocity;
        [SerializeField]
        [Tooltip("How quickly to speed the player up before they hit max speed")]
        private float _givenAccel;
        [SerializeField]
        [Tooltip("How quickly to slow the player down before stopping.")]
        private float _givenDecel;
        [SerializeField]
        [Tooltip("The speed at which the player should slide down a wall.")]
        private float _wallSlideSpeed;
        [SerializeField]
        [Tooltip("Max horizontal velocity in the air.")]
        private float _maxHorizontalAirSpeed;
        [SerializeField]
        [Tooltip("The force with which to shoot the grappling hook while in this state")]
        private float _hookShotForce;
        /// <summary>
        /// Whether or not the player entered this state with a jump.
        /// </summary>
        private bool _continuingJumpFromPrevState;
        private bool _grappleInput;
        new public States Name => States.Airborne;
        private bool _queueGroundJump = false, _hasJumpEnded = true;
        private int _queueWallJump = 0;
        public float WallSlideSpeed => _wallSlideSpeed;
        private float _gravityScale, _horizontalInput, _acceleration, _deceleration;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
            _gravityScale = _rb.gravityScale;
        }
        public override void Enter() {
            base.Enter();
            HandleInput();

            _continuingJumpFromPrevState = _rb.velocity.y > 1;
            _hasJumpEnded = !_continuingJumpFromPrevState;
        }
        public override void Exit() {
            base.Exit();
            _rb.gravityScale = _gravityScale;
        }
        protected override void HandleInput() {
<<<<<<< Updated upstream
            if(Input.GetButton("Down")) {
=======
            if(_input.y < 0) {
>>>>>>> Stashed changes
                _sm.BufferInput("Down", 0.1f);
            }

            if (Input.GetButtonDown("Grapple")) {
                _grappleInput = true;
            }

            #region Jump
            if (_input.y > 0) {
                if (_continuingJumpFromPrevState && _stateEnterTime + _jumpCoyoteTime > Time.time) {
                    //Continue a jump from the previous state
                    _queueGroundJump = true;
                } else if (WallCheck() != 0) {
                    //Wall jump
                    _queueWallJump = WallCheck();
                } else {
                    //Buffer the jump input
                    _sm.BufferInput("Jump", 0.1f);
                }
            } else {
                //Stop the jump from the previous state
                _continuingJumpFromPrevState = false;
                _queueGroundJump = false;
                _queueWallJump = 0;
            }
            #endregion

            #region Air Control
            //TODO: Implement air control
            //calculates direction to move in and desired velocity
                float targetSpeed = _input.x * _maxHorizontalAirSpeed;
                float speedDif = 0;
                if (IsPlayerSpeedExceeding(targetSpeed))
                {
                    speedDif = -1 * Mathf.Sign(_rb.velocity.x);
                }
                else
                {
                    //calculates difference between current velocity and desired velocity
                    speedDif = targetSpeed - _rb.velocity.x;
                }
                //change acceleration rate depending on the situation
                //when target speed is > 0.01f, use acceleration variable, else use deceleration variable
                //_accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
                //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
                //finally multiplies by sing to reapply direction
                //_horizontalMovement = Mathf.Pow(Mathf.Abs(speedDif) * _accelRate, _velPower) * Mathf.Sign(speedDif);
            #endregion

            if (_uncheckedInputBuffer) {
                _uncheckedInputBuffer = false;
                CheckInputBuffer();
            }

        }
        protected override void LogicUpdate() {
            if (!_hasJumpEnded && _rb.velocity.y < 0) {
                _hasJumpEnded = true;
                OnJumpEnd();
            }

            if (IsGrounded) {
                _transitionToState = _sm.CheckBufferedInputsFor("Down") ? States.Sliding : States.Running;
            } else if (_hook.IsAttached) {
                _transitionToState = States.Grappling;
            }
        }
        protected override void PhysicsUpdate() {
            #region Jump Gravity
            if(_rb.velocity.y < 0)
            {
                _rb.gravityScale = _gravityScale * _fallGravityMultiplier;
            }
            else
            {
                _rb.gravityScale = _gravityScale;
            }
            #endregion

            #region Terminal Velocity
            if(_rb.velocity.y < -_terminalVelocity)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_terminalVelocity);
            }
            #endregion

            #region Wall Interaction
            if (WallCheck() != 0) {
                    _acceleration = 0;
                    _deceleration = 0;
            

                //wall friction
                if (_rb.velocity.y < -_wallSlideSpeed)
                {
                    if (_input.x * WallCheck() > 0.01f)
                    {
                        _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlideSpeed);
                    }
                }
            } else
            {
                _acceleration = _givenAccel;
                _deceleration = _givenDecel;
            }
            #endregion

            if(_grappleInput) {
                HandleGrappleInput(_input, _hookShotForce);
                _grappleInput = false;
            }

            //continuing a ground jump
            if (_queueGroundJump) {
                GroundedJump();
                _queueGroundJump = false;
                return;
            }
            
            //wall jumping
            if (_queueWallJump != 0) {
                WallJump(_queueWallJump);
                _queueWallJump = 0;
                return;
            }
        }
        protected override void CheckInputBuffer()
        {
            throw new System.NotImplementedException();
        }

        private void OnJumpEnd()
        {
            if(_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _jumpCutMultiplier), ForceMode2D.Impulse);
            }

            //jumpInputReleased = true;
            //lastJumpTime = 0;
        }

        /// <summary>
        /// Jumps the player off of a wall.
        /// </summary>
        /// <param name="wallSide">The side of the player the wall is located to. -1 for the player's left. 1 for the player's right</param>
        private void WallJump(int wallSide)
        {
            if (wallSide == 0) return;
            if (_rb.velocity.y < 0)
            {
                _rb.velocity = new Vector2(0, 0);
            }
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            _rb.AddForce(new Vector2(-1 * wallSide, 1) * _wallJumpForce, ForceMode2D.Impulse);
        }
    }
}