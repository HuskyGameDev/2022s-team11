using System;
using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
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
        #endregion

        #region Variables
        private bool _isJumpingInput, _isCrouchingInput, _isGrappleInput;
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
        }
        public override void Exit() {
            base.Exit();
        }
        #endregion
        #region MovementState Overrides
        protected override void HandleInput() {
            var gameTime = Time.time;
            _input = GetInput();
            _isJumpingInput = _input.y > 0;
            _isCrouchingInput = _input.y < 0;
            _isGrappleInput = Input.GetButton("Grapple");
            
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
                _transitionToState = States.Airborne;
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
            
            if (_isJumpingInput) GroundedJump();
        }

        protected override void CheckInputBuffer() {
            _isJumpingInput = _isJumpingInput || _sm.CheckBufferedInputsFor("Jump");
            _isCrouchingInput = _isCrouchingInput || _sm.CheckBufferedInputsFor("Slide");
            _uncheckedInputBuffer = false;
        }
        #endregion

    }
}