using System;
using UnityEngine;

namespace Movement
{
    /** Author: Nick Zimanski
    * Version 3/21/22
    */
    [CreateAssetMenu(fileName = "SlidingStateData", menuName = "ScriptableObjects/MovementStates/SlidingStateScriptableObject")]
    public class SlidingState : MovementState
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
        [Tooltip("How much force to apply to the player as they slide.")]
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
        private bool _isCrouchingInput, _grappleInput;
        private float _movement, _accelRate, _acceleration, _deceleration;
        new public States Name => States.Sliding;
        #endregion

        public override void Initialize(GameManager game, PlayerController player, MovementStateMachine sm)
        {
            base.Initialize(game, player, sm);
        }

        public override void Enter()
        {
            HandleInput();
            base.Enter();
            _acceleration = _givenAccel;
            _deceleration = _givenDecel;
        }
        public override void Exit()
        {
            base.Exit();
        }
        protected override void HandleInput()
        {
            var gameTime = Time.time;
            _isCrouchingInput = _gm.DirectionalInput.y < 0;

            if (_gm.inputManager.GetButtonDown("Grapple"))
            {
                _grappleInput = true;
                _sm.BufferInput("Grapple", 0.1f);
            }

            if (_gm.inputManager.GetButtonDown("Jump"))
            {
                _sm.BufferInput("Jump", _jumpBufferTime);
                Debug.Log("Jump buffered");
            }

            if (_uncheckedInputBuffer)
            {
                CheckInputBuffer();
                _uncheckedInputBuffer = false;
            }

        }
        protected override void LogicUpdate()
        {
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
            _accelRate = _deceleration;
            //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
            //finally multiplies by sing to reapply direction
            _movement = Mathf.Pow(Mathf.Abs(speedDif) * _accelRate, _velPower) * Mathf.Sign(speedDif);

            if (!IsGrounded)
            {
                _transitionToState = States.Airborne;
            }
            else if (_hook.IsAttached)
            {
                _transitionToState = States.Grappling;
            }
            else if (!_gm.inputManager.GetButton("Slide"))
            {
                _transitionToState = States.Running;
            }
        }
        protected override void PhysicsUpdate()
        {
            _rb.AddForce(_movement * Vector2.right);

            if (Mathf.Abs(_gm.DirectionalInput.x) < 0.01f)
            {
                float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
                amount *= Mathf.Sign(_rb.velocity.x);
                _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
        }
        protected override void CheckInputBuffer()
        {
            _uncheckedInputBuffer = false;
        }
    }
}