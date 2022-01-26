using System;
using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    [CreateAssetMenu(fileName = "RunningStateData", menuName = "ScriptableObjects/RunningStateScriptableObject")]
    public class RunningState : GroundedState
    {
        #region Variables
        [Header("Movement")]
        [SerializeField]private float _givenAccel;
        [SerializeField]private float _givenDecel, _frictionAmount, _velPower, _maxHorizontalSpeed;
        [Header("Jump")]
        [SerializeField]private float _jumpCoyoteTime;
        private float _horizontalInput, _acceleration, _deceleration, _movement, _accelRate;
    
        #endregion
        public override void Initialize(_Scripts.Managers.PlayerManager player, _Scripts.Utility.StateMachine<MovementState> sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            base.Enter();
        }
        public override void Exit() {
            base.Exit();
        }
        public override void HandleInput() {
            
            _horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        public override void LogicUpdate() {
            #region Normal Movement
                //calculates direction to move in and desired velocity
                float targetSpeed = _horizontalInput * _maxHorizontalSpeed;
                float speedDif;
                if (Exceeding(targetSpeed) && _owner.LastGroundedTime < _jumpCoyoteTime - .01f)
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

            #region Friction
                if (_owner.LastGroundedTime > 0 && Mathf.Abs(_horizontalInput) < 0.01f && _owner.LastWallJump < 0)
                {
                    float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
                    amount *= Mathf.Sign(_rb.velocity.x);
                    _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
                }
            #endregion
        }
        public override void PhysicsUpdate() {
            //applies force to rigidbody, multiplying by Vector2.right so that it only affects X axis
            _rb.AddForce(_movement * Vector2.right);
        }

        private bool Exceeding(float v)
        {
            if(Mathf.Abs(_rb.velocity.x) > Mathf.Abs(v) && Mathf.Abs(v) > 0.1f && Mathf.Sign(v) == Mathf.Sign(_rb.velocity.x))
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}