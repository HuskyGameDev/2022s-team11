using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 1/26/22
    */
    [CreateAssetMenu(fileName = "SlidingStateData", menuName = "ScriptableObjects/MovementStates/SlidingStateScriptableObject")]
    public class SlidingState : MovementState
    {
        new public States Name => States.Sliding;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            base.Enter();
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
        protected override void CheckInputBuffer()
        {
            throw new System.NotImplementedException();
        }
    }
}