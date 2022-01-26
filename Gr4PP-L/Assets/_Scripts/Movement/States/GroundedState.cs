using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    [CreateAssetMenu(fileName = "GroundedStateData", menuName = "ScriptableObjects/GroundedStateScriptableObject")]
    public class GroundedState : MovementState
    {
        protected _Scripts.Managers.PlayerManager _owner;
        private bool _runningRight, _runningLeft, _jumping, _crouching;
        public override void Initialize(_Scripts.Managers.PlayerManager player, _Scripts.Utility.StateMachine<MovementState> sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            _owner.IsGrounded = true;
        }
        public override void Exit() {
            _owner.IsGrounded = false;
        }
        public override void HandleInput() {
            _runningRight = Input.GetButton("Right");
            _runningLeft = Input.GetButton("Left");
            _jumping = Input.GetButton("Jump");
            _crouching = Input.GetButton("Slide");
        }
        public override void LogicUpdate() {
            #region State changes
            if(_runningLeft || _runningRight) {
                if (_crouching) {
                    _sm.ChangeState(_sm.GetState((int) States.Sliding));
                    return;
                }
                _sm.ChangeState(_sm.GetState((int) States.Running));
                return;
            }
            if (_jumping) {
                _sm.ChangeState(_sm.GetState((int) States.Airborne));
                return;
            }
            #endregion
        }
        public override void PhysicsUpdate() {

        }
    }
}
