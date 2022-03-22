using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 3/21/22
    */
    [CreateAssetMenu(fileName = "SlidingStateData", menuName = "ScriptableObjects/MovementStates/SlidingStateScriptableObject")]
    public class SlidingState : MovementState
    {
        [SerializeField]
        [Tooltip("The force with which to shoot the grappling hook while in this state")]
        private float _hookShotForce;
        new public States Name => States.Sliding;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
        }
        public override void Enter() {
            HandleInput();
            base.Enter();
        }
        public override void Exit() {
            base.Exit();
        }
        protected override void HandleInput() {

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