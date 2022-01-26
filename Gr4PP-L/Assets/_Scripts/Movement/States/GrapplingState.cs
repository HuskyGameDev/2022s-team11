using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    [CreateAssetMenu(fileName = "GrapplingStateData", menuName = "ScriptableObjects/GrapplingStateScriptableObject")]
    public class GrapplingState : AirborneState
    {
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

        }
        public override void LogicUpdate() {

        }
        public override void PhysicsUpdate() {

        }
    }
}