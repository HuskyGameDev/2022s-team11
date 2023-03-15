using System;
using UnityEngine;

namespace Movement
{
    /** Author: Nick Zimanski & Noah Kolczynski
    * Version 11/29/22
    */
    [CreateAssetMenu(fileName = "RunningStateData", menuName = "ScriptableObjects/MovementStates/RunningStateScriptableObject")]
    //idea: change player's velocity when they enter and when they exit the trigger.
    //specifically, reduce the velocity by the "velocity" of the platform on enter and increase the velocity by the velocity of the platform on exit
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
        [Header("Movement")]
        [SerializeField]
        [Tooltip("How much time, in seconds, between step sound effects while moving")]
        private float _stepSoundInterval = 0.5f;
        #endregion

        #region Variables
        private bool _isJumpingInput, _isCrouchingInput, _grappleInput;
        private float _movement, _accelRate, _acceleration, _deceleration, _lastStepSoundTime;
        private bool _isMoving => _rb.velocity.magnitude > 0.1f;
        new public States Name => States.Running;
        #endregion

        #region MovementState Callbacks
        public override void Initialize(GameManager game, Movement.PlayerController player, MovementStateMachine sm)
        {
            base.Initialize(game, player, sm);
        }
        public override void Enter()
        {
            base.Enter();
            _acceleration = _givenAccel;
            _deceleration = _givenDecel;
            HandleInput();
            if (!_owner.CanGrapple) _owner.CanGrapple = !_sm.CheckBufferedInputsFor("WallTouchTransition");
            _lastStepSoundTime = 0f;
        }
        public override void Exit()
        {
            base.Exit();
        }
        #endregion


        #region MovementState Overrides
        protected override void HandleInput()
        {
            var gameTime = Time.time;
            _isCrouchingInput = _gm.DirectionalInput.y < 0;

            if (_gm.Get<Managers.InputManager>().GetButtonDown("Fire") && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
            {
                Debug.Log("Should Grapple");
                _owner.CanGrapple = true;
                _grappleInput = true;
                _sm.BufferInput("Fire", 0.1f);
            }

            if (_gm.Get<Managers.InputManager>().GetButtonDown("Jump"))
            {
                _sm.BufferInput("Jump", _jumpBufferTime);
            }

            if (_uncheckedInputBuffer)
            {
                CheckInputBuffer();
                _uncheckedInputBuffer = false;
            }
        }
        protected override void LogicUpdate()
        {
            #region Normal Movement
            //calculates direction to move in and desired velocity
            float targetSpeed = _gm.DirectionalInput.x * _maxHorizontalSpeed;
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
            if (targetSpeed == 0) _accelRate = 0;
            else _accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
            //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
            //finally multiplies by sing to reapply direction
            _movement = Mathf.Pow(Mathf.Abs(speedDif) * _accelRate, _velPower) * Mathf.Sign(speedDif);

            #endregion

            #region Sound
            if (_isMoving && _lastStepSoundTime + _stepSoundInterval < Time.time)
            {
                //TODO: Play different sounds for material beneath player
                _gm.Get<Managers.AudioManager>().PlayVariantPitch("Metal Footstep " + GameManager.Random.Next(2));
                _lastStepSoundTime = Time.time;
            }
            #endregion

            #region State Checks
            if (!IsGrounded && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
            {
                _sm.BufferInput("Ground to Air", 0.1f);
                _owner.CanGrapple = true;
                _transitionToState = States.Airborne;
                return;
            }

            if (_hook.IsAttached)
            {
                _sm.RemoveBufferedInputsFor("fire");
                _owner.CanGrapple = false;
                _transitionToState = States.Grappling;
                return;
            }

            if (Mathf.Abs(_rb.velocity.x) <= _maxHorizontalSpeed + 0.01f) return;
            if ((_gm.Get<Managers.InputManager>().GetButton("Slide") || _gm.DirectionalInput.y < 0) && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
            {
                _owner.CanGrapple = true;
                _transitionToState = States.Sliding;
                return;
            }
            #endregion
        }
        protected override void PhysicsUpdate()
        {
            //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis
            if (_movement != 0) _rb.AddForce(_movement * Vector2.right);


            #region Friction
            if (Mathf.Abs(_gm.DirectionalInput.x) < 0.01f)
            {
                float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
                amount *= Mathf.Sign(_rb.velocity.x);
                _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
            #endregion

            if (_sm.CheckBufferedInputsFor("Jump") && (GroundCollider() == null || !GroundCollider().CompareTag("Jump Pad")))
            {
                // this ugly if statement checks to see if the player is either not touching a wall, not holding a direction, or touching the wall, but holding in the direction of the wall.
                // this allows the player while grounded to jump up the side of a wall if they're touching it.
                // the second line of the if statement ensures that the player only gets a grounded jump when touching the wall if they've been in the grounded state for more than 0.1 seconds.
                // this ensures that, should the player clip into the wall momentarily when trying to wall jump, they don't get a grounded jump.
                if ((WallCheck() == 0 || _gm.DirectionalInput.x == 0 || (WallCheck() != 0 && Mathf.Sign(WallCheck()) == Mathf.Sign(_gm.DirectionalInput.x)))
                    && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
                {
                    _owner.CanGrapple = true;
                    GroundedJump();
                }
                else
                {
                    _transitionToState = States.Airborne;
                }
            }

            if (_sm.CheckBufferedInputsFor("fire") && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
            {
                _owner.CanGrapple = true;
                HandleGrappleInput(_gm.DirectionalInput, _hookShotForce);
                _grappleInput = false;
            }
        }

        protected override void CheckInputBuffer()
        {
            _isCrouchingInput = _isCrouchingInput || _sm.CheckBufferedInputsFor("Slide");
            _uncheckedInputBuffer = false;
        }
        #endregion

    }
}