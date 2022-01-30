using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    [CreateAssetMenu(fileName = "GrapplingStateData", menuName = "ScriptableObjects/MovementStates/GrapplingStateScriptableObject")]
    public class GrapplingState : AirborneState
    {
        new public States Name => States.Grappling;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            base.Enter();
            _transitionToState = States.Running;
        }
        public override void Exit() {
            base.Exit();
        }
        protected override void HandleInput() {
            _input = GetInput();

        }
        protected override void LogicUpdate() {

        }
        protected override void PhysicsUpdate() {

        }
    }
}