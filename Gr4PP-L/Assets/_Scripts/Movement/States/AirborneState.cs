using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 3/21/22
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
        [SerializeField]
        [Tooltip("How long after a wall jump before the player regains control of the character")]
        private float _wallJumpPreservationTime;
        [SerializeField]
        [Tooltip("How long between a grounded jump and a wall jump is acceptable")]
        private float _jumpWallJumpSpacing;
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
        [Tooltip("Helps smooth movement, exponent used for movement calculations.")]
        private float _velPower;
        [SerializeField]
        [Tooltip("The speed at which the player should slide down a wall.")]
        private float _wallSlideSpeed;
        [SerializeField]
        [Tooltip("Max horizontal velocity in the air.")]
        private float _maxHorizontalAirSpeed;
        
        //variables used for calculating 
        private float _horizontalMovement, _accelRate;

        // used to prevent player from moving back towards the wall for a short period of time
        private float _lastWallJump;

        // tracks if the jump button is pressed
        private bool _jumpPressed;

        // used to call a grounded jump when player presses jump shortly after touching ground
        private bool _queueCoyoteJump;

        [SerializeField]
        [Tooltip("The force with which to shoot the grappling hook while in this state")]
        private float _hookShotForce;
        /// <summary>
        /// Whether or not the player entered this state with a jump.
        /// </summary>
        private bool _continuingJumpFromPrevState;
        private bool _grappleInput;
        new public States Name => States.Airborne;
        private bool _hasJumpEnded = false, _jumpEndCalled = false;
        private int _queueWallJump = 0;
        public float WallSlideSpeed => _wallSlideSpeed;
        private float _gravityScale, _horizontalInput, _acceleration, _deceleration;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm) {
            base.Initialize(player, sm);
            _gravityScale = _rb.gravityScale;
        }
        public override void Enter() {
            base.Enter();
            HandleInput();

            _gravityScale = _rb.gravityScale;
            _jumpEndCalled = false;
            _lastWallJump = -1;

            _hasJumpEnded = !(_sm.CheckBufferedInputsFor("Grounded Jump"));
        }
        public override void Exit() {
            base.Exit();
            _rb.gravityScale = _gravityScale;
        }
        protected override void HandleInput() {
            if(_input.y < 0) {
                _sm.BufferInput("Down", 0.1f);
            }
            if (Input.GetButtonDown("Jump")) {
                _sm.BufferInput("Jump", _jumpBufferTime);
            }

            _jumpPressed = Input.GetButton("Jump");
            
            if (Input.GetButtonDown("Grapple")) {
                _sm.BufferInput("Grapple", 0.1f);
                _grappleInput = true;
            }

            if (_uncheckedInputBuffer) {
                _uncheckedInputBuffer = false;
                CheckInputBuffer();
            }
        }

        protected override void LogicUpdate() {
            if (IsGrounded && ((_sm.CheckBufferedInputsFor("Jump") == false) || (_sm.CheckBufferedInputsFor("Jump") == true && WallCheck() == 0))) {
                if(WallCheck() != 0) {
                    _sm.BufferInput("WallTouchTransition", 0.05f);
                }
                _transitionToState = _sm.CheckBufferedInputsFor("Down") ? States.Sliding : States.Running;
            } /**else if (!_owner.IsGrappleHeld) {
                _transitionToState = States.Grappling;
            }*/

            if(!_jumpPressed && !_hasJumpEnded) {
                _jumpEndCalled = true;
            }

            #region Jump
            if (WallCheck() != 0 && _sm.CheckBufferedInputsFor("Jump")) {
                _queueWallJump = WallCheck();
            }
            if(WallCheck() == 0 && _sm.CheckBufferedInputsFor("Jump") && _stateEnterTime > Time.time - _jumpCoyoteTime && _lastWallJump < 0 && _sm.CheckBufferedInputsFor("Ground to Air")) {
                _queueCoyoteJump = true;
            }
            #endregion

            #region Air Control
            //calculates direction to move in and desired velocity
            if (_lastWallJump < 0) {
                float targetSpeed = _input.x * _maxHorizontalAirSpeed;
                float speedDif = 0;
                if (IsPlayerSpeedExceeding(targetSpeed)) {
                    //when the character is exceeding our maximum velocity, speed dif will have a value of 1 in the opposite horizontal direction
                    //so if the character is going fast to the right, this will give a -1, if going fast to the left, it'll give a 1
                    speedDif = -1 * Mathf.Sign(_rb.velocity.x);
                } else {
                    //calculates difference between current velocity and desired velocity
                    speedDif = targetSpeed - _rb.velocity.x;
                }
                //change acceleration rate depending on the situation
                //when target speed is > 0.01f, use acceleration variable, else use deceleration variable
                _accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
                //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
                //finally multiplies by sing to reapply direction
                _horizontalMovement = Mathf.Pow(Mathf.Abs(speedDif) * _accelRate, _velPower) * Mathf.Sign(speedDif);
            } else {
                _horizontalMovement = 0;
            }
            #endregion

            #region Cooldown Timers
            if (_lastWallJump > -1) {
                _lastWallJump -= Time.deltaTime;
            }
            #endregion

            if (_hook.IsAttached) {
                _sm.RemoveBufferedInputsFor("Grapple");
                _owner._canGrapple = false;
                _transitionToState = States.Grappling;
            }
        }
        protected override void PhysicsUpdate() {
            #region Horizontal Movement
            //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis
            _rb.AddForce(_horizontalMovement * Vector2.right);
            #endregion

            #region Jump Cut
            if(!_hasJumpEnded && _jumpEndCalled) {
                OnJumpEnd();
                _hasJumpEnded = true;
            }
            #endregion

            #region Jump Gravity
            if (_rb.velocity.y < 0)
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
                //_rb.velocity = new Vector2(_rb.velocity.x, -_terminalVelocity);
                _rb.gravityScale = _gravityScale * 0.5f;
            }
            #endregion

            #region Wall Interaction
            if (WallCheck() != 0) {
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

            if (_grappleInput) {
                HandleGrappleInput(_input, _hookShotForce);
                _grappleInput = false;    
            }
            
            #region WallJump
            if (_queueWallJump != 0) {
                WallJump(_queueWallJump);
                _queueWallJump = 0;
                return;
            }
            #endregion

            #region CoyoteJump
            if (_queueCoyoteJump) {
                _queueCoyoteJump = false;
                _hasJumpEnded = false;
                GroundedJump();
            }
            #endregion
        }
        protected override void CheckInputBuffer()
        {
            _uncheckedInputBuffer = false;
        }

        /// <summary>
        /// Cuts player's jump short.
        /// </summary>
        private void OnJumpEnd()
        {
            if(_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _jumpCutMultiplier), ForceMode2D.Impulse);
            }

            _hasJumpEnded = true;
            _jumpEndCalled = false;
            _sm.RemoveBufferedInputsFor("Jumped");
            //jumpInputReleased = true;
            //lastJumpTime = 0;
        }

        /// <summary>
        /// Jumps the player off of a wall.
        /// </summary>
        /// <param name="wallSide">The side of the player the wall is located to. -1 for the player's left. 1 for the player's right</param>
        private void WallJump(int wallSide)
        {
            if (wallSide == 0 || _lastWallJump > 0.5 * _wallJumpPreservationTime){
                return;
            }
            _lastWallJump = _wallJumpPreservationTime;
            if (_rb.velocity.y < _wallJumpForce * 0.5 || _sm.CheckBufferedInputsFor("Ground Jump")){
                _rb.velocity = new Vector2(0, 0);
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _rb.AddForce(new Vector2(-1 * wallSide, 1) * _wallJumpForce, ForceMode2D.Impulse);
            } else {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _rb.AddForce(new Vector2(-1 * wallSide, 0.5f) * _wallJumpForce, ForceMode2D.Impulse);
            }
            
            _sm.RemoveBufferedInputsFor("Jump");
            _hasJumpEnded = !_jumpPressed;
        }
    }
}