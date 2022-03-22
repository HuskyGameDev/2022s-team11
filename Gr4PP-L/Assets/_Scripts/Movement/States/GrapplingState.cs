
using UnityEngine;
namespace _Scripts.Movement.States {
    /** Author: Nick Zimanski
    * Version 2/20/22
    */
    [CreateAssetMenu(fileName = "GrapplingStateData", menuName = "ScriptableObjects/MovementStates/GrapplingStateScriptableObject")]
    public class GrapplingState : MovementState
    {
        #region Variables
        private GrappleHookController _hookController;
        private Rigidbody2D _hookRb;
        [SerializeField]
        private float _grappleFireForce;
        [SerializeField]
        private float _horizPullStrength;
        [SerializeField]
        private float _vertPullStrength;
        [SerializeField]
        private float _grappleTimeOut;
        [SerializeField]
        [Range(0,1)]
        private float _downwardPullMagnitude;
        [SerializeField]
        [Tooltip("The percentage of the force vector to change the pull by according to the player's input")]
        [Range(0,1)]
        private float _playerInputMagnitude;
        [SerializeField]
        [Range(1,3)]
        private float _tensionSpeedMultiplier;
        private float _lastDistance = 0;
        private bool _grappleInput = false;
        private Vector2 newVel;
        private Vector2 oldVel;
        #endregion
        new public States Name => States.Grappling;
        public override void Initialize(_Scripts.Managers.PlayerManager player, MovementStateMachine sm)
        {
            base.Initialize(player, sm);
            _hookController = player.GrappleHookCtrl;
            _hookRb = player.GrappleHookRigidbody;
        }
        public override void Enter() {
            base.Enter();
            HandleInput();
            _lastDistance = 0;
        }
        public override void Exit() {
            base.Exit();
            _hookController.RetractHook();
        }
        protected override void HandleInput() {
            _grappleInput = Input.GetButtonDown("Grapple");

            if (_input.y < 0) {
                _sm.BufferInput("Down", 0.1f);
            }

            if (_uncheckedInputBuffer) {
                CheckInputBuffer();
                _uncheckedInputBuffer = false;
            }
        }
        //TODO: MAKE GRAPPLE FIRE MEMBER OF MOVEMENT STATE
        //TODO: ONLY ENTER GRAPPLE STATE ONCE HOOK CONNECTS
        protected override void LogicUpdate() {

            //Check for retraction
            if (_grappleInput) {
                _hookController.RetractHook();
                if (IsGrounded) {
                    _transitionToState = States.Running;
                } else {
                    _transitionToState = States.Airborne;
                }
            }


            //if (_stateEnterTime + _grappleTimeOut > Time.time) return;

            if (_hookController.IsAttached) return;

            //Hook hasn't hit anything in the timeout window -- change state
            if (IsGrounded) {
                _transitionToState = States.Running;
            } else {
                _transitionToState = States.Airborne;
            }
        }
        protected override void PhysicsUpdate() {
            if (!_grappleInput && !_hookController.IsAttached) return;

            if (_grappleInput) {
                //Fire the hook
                //if (_hookController.IsHeld) _hookController.FireHook(_input, _grappleFireForce);
                //Retract the hook
                //else _hookController.RetractHook();
                return;
            }

            if (_hookController.IsAttached) {
                Grapple();
                return;
            }
        }
        protected override void CheckInputBuffer() {
            //_grappleInput = _sm.CheckBufferedInputsFor("Grapple") || _grappleInput;
        }
        private void Grapple() {
            var tetherVector = (_hookController.TetherPosition - _hookController.Position) * -1;
            var distance = Mathf.Abs(tetherVector.magnitude);
            if (_lastDistance == 0) _lastDistance = distance;
            var tetherPlayerDifference = _rb.position - _hookController.TetherPosition;

            var pullVector = tetherVector;

            var playerVelocity = _rb.velocity;
            //if (playerVelocity.mag) {
            //    Debug.Log("Moving player back...");
            //    _rb.MovePosition(_hookController.Position + Vector2.ClampMagnitude(tetherVector, _lastDistance) - tetherPlayerDifference);
            //} //Stop player from going beyond grapple length
            
            //Pull player downward
            pullVector = tetherVector.y < 0 ? pullVector * new Vector2(1, _downwardPullMagnitude) : pullVector;

            //Factor in player input
            pullVector += new Vector2(_input.x * Mathf.Abs(pullVector.x), _input.y * Mathf.Abs(pullVector.y)) * _playerInputMagnitude;

            //Constrain player to end of rope "circle"
            //Look forward to see if the player will break the circle
            /**oldVel = (_hookController.TetherPosition + playerVelocity * Time.fixedDeltaTime);
            newVel = playerVelocity;
            if ((oldVel + tetherVector).magnitude > distance) {
                newVel = Vector2.ClampMagnitude(oldVel + tetherVector, distance);
            }

            
            //Reassign velocity
            _rb.velocity = newVel;**/

            pullVector = (pullVector - tetherPlayerDifference).normalized;
            Debug.Log(new Vector2(pullVector.x * _horizPullStrength, pullVector.y * _vertPullStrength));
            _rb.AddForce(new Vector2(pullVector.x * _horizPullStrength, pullVector.y * _vertPullStrength) , ForceMode2D.Force);
            
            _lastDistance = distance < _lastDistance ? distance : _lastDistance;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_hookController.TetherPosition, oldVel);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_hookController.TetherPosition, newVel);
        }
    }
}