using System;
using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski && Noah Kolczynski
    * Version 3/21/22
    */
    [CreateAssetMenu(fileName = "RunningStateData", menuName = "ScriptableObjects/MovementStates/RunningStateScriptableObject")]
    public class RunningState : MovementState
    {
        #region Serialized Variables
        [Header("Movement")]
        [SerializeField]
        [Tooltip("How quickly to speed the player up before they reach max speed.")]
        private float _givenAccel;
        [SerializeField]
        [Tooltip("How quickly to slow the player down before they stop.")]
        private float _givenDecel;
        [SerializeField]
        [Tooltip("How much force to apply to the player as they move.")]
        private float _frictionAmount;
        [SerializeField]
        [Tooltip("The power to raise the player's acceleration to.")]
        private float _velPower;
        [SerializeField]
        [Tooltip("The maximum speed the player can normally reach horizontally.")]
        private float _maxHorizontalSpeed;
        [SerializeField]
        [Tooltip("The length of time a jump input will be usable in the buffer")]
        private float _jumpBufferTime;
        [SerializeField]
        [Tooltip("The force with which to shoot the grappling hook while in this state")]
        private float _hookShotForce;
        #endregion

        #region Variables
        private bool _isJumpingInput, _isCrouchingInput, _grappleInput;
        private float _movement, _accelRate, _acceleration, _deceleration;
        new public States Name => States.Running;
        #endregion

        #region MovementState Callbacks
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            base.Enter();
            _acceleration = _givenAccel;
            _deceleration = _givenDecel;
            HandleInput();
            _owner._canGrapple = true;
        }
        public override void Exit() {
            base.Exit();
        }
        #endregion
        #region MovementState Overrides
        protected override void HandleInput() {
            var gameTime = Time.time;
            _input = GetInput();
            _isCrouchingInput = _input.y < 0;
            
            if (Input.GetButtonDown("Grapple")) {
                _grappleInput = true;
                _sm.BufferInput("Grapple", 0.1f);
            }

            if (Input.GetButtonDown("Jump")) {
                _sm.BufferInput("Jump", _jumpBufferTime);
            }

            if (_uncheckedInputBuffer) {
                CheckInputBuffer();
                _uncheckedInputBuffer = false;
            }
        }
        protected override void LogicUpdate() {
            #region Normal Movement
                //calculates direction to move in and desired velocity
                float targetSpeed = _input.x * _maxHorizontalSpeed;
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
                _accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
                //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
                //finally multiplies by sing to reapply direction
                _movement = Mathf.Pow(Mathf.Abs(speedDif) * _accelRate, _velPower) * Mathf.Sign(speedDif);

            #endregion

            #region StateChecks
            if (!IsGrounded) {
                _sm.BufferInput("Ground to Air", 0.1f);
                _transitionToState = States.Airborne;
            } else if (_hook.IsAttached) {
                _sm.RemoveBufferedInputsFor("Grapple");
                _owner._canGrapple = false;
                _transitionToState = States.Grappling;
            } else if (Input.GetButton("Slide"))
            {
                _transitionToState = States.Sliding;
            }
            #endregion
        }
        protected override void PhysicsUpdate() {
            //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis
            _rb.AddForce(_movement * Vector2.right);

            #region Friction
                if (Mathf.Abs(_input.x) < 0.01f)
                {
                    float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
                    amount *= Mathf.Sign(_rb.velocity.x);
                    _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }
            #endregion

            if (_sm.CheckBufferedInputsFor("Jump") && !GroundCollider().CompareTag("Jump Pad")) {
                // this ugly if statement checks to see if the player is either not touching a wall, not holding a direction, or touching the wall, but holding in the direction of the wall.
                // this allows the player while grounded to jump up the side of a wall if they're touching it.
                // the second line of the if statement ensures that the player only gets a grounded jump when touching the wall if they've been in the grounded state for more than 0.1 seconds.
                // this ensures that, should the player clip into the wall momentarily when trying to wall jump, they don't get a grounded jump.
                if ((WallCheck() == 0 || _input.x == 0 ||(WallCheck() != 0 && Mathf.Sign(WallCheck()) == Mathf.Sign(_input.x)))
                    && !_sm.CheckBufferedInputsFor("WallTouchTransition")) {
                    GroundedJump();
                } else {
                    _sm.RemoveBufferedInputsFor("WallTouchTransition");
                    _transitionToState = States.Airborne;
                }
            }

            if(_sm.CheckBufferedInputsFor("Grapple")) {
                HandleGrappleInput(_input, _hookShotForce);
                _grappleInput = false;
            }
        }

        protected override void CheckInputBuffer() {
            _isCrouchingInput = _isCrouchingInput || _sm.CheckBufferedInputsFor("Slide");
            _uncheckedInputBuffer = false;
        }
        #endregion

    }
}