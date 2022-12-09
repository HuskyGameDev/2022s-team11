using System;
using UnityEngine;

namespace Movement
{
    /** Author: Nick Zimanski
    * Version 11/29/22
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
        private bool _isCrouchingInput, _jumpInput;
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

            _gm.Get<Managers.AudioManager>().Play("Slide");
            _acceleration = _givenAccel;
            _deceleration = _givenDecel;
        }
        public override void Exit()
        {
            base.Exit();
            _gm.Get<Managers.AudioManager>().Stop("Slide");
        }
        protected override void HandleInput()
        {
            var gameTime = Time.time;
            _isCrouchingInput = _gm.Get<Managers.InputManager>().GetButton("Slide") || _gm.DirectionalInput.y < 0;

            if (_gm.Get<Managers.InputManager>().GetButtonDown("Grapple"))
            {
                _sm.BufferInput("Grapple", 0.1f);
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
            _movement = 0f;

            #region State Checks
            if (!IsGrounded && !_sm.CheckBufferedInputsFor("WallTouchTransition"))
            {
                _transitionToState = States.Airborne;
                return;
            }

            if (_hook.IsAttached)
            {
                _transitionToState = States.Grappling;
                return;
            }

            if (!(_isCrouchingInput) || Mathf.Abs(_rb.velocity.x) < 0.01f)
            {
                _transitionToState = States.Running;
                return;
            }
            #endregion
        }
        protected override void PhysicsUpdate()
        {
            CheckInputBuffer();
            //_rb.AddForce(_movement * Vector2.right);

            float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
            amount *= Mathf.Sign(_rb.velocity.x);
            _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);


            if (_jumpInput)
            {
                _jumpInput = false;
                _owner.CanGrapple = true;
                GroundedJump();
            }
        }
        protected override void CheckInputBuffer()
        {
            _jumpInput = _sm.CheckBufferedInputsFor("Jump");
            _uncheckedInputBuffer = false;
        }
    }
}