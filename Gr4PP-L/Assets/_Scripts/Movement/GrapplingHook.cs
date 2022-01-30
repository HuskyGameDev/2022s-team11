using System;
using UnityEngine;

namespace _Scripts.Movement {
    public class GrapplingHook : MonoBehaviour
    {
        //TODO: PORT TO MOVEMENT STATE SYSTEM
        /**private float _screenWidth, _screenHeight;
        [SerializeField]private GameObject _grappleHinge;
        private Rigidbody2D _grappleHingeRigidbody;
        [SerializeField]private Rigidbody2D _playerRigidbody;
        [SerializeField]private GameObject _grappleTether;
        [SerializeField]private float _rampingPullIncrement=.1f, _rampingPullValue=1, _grappleHookTravelSpeedModifier = 100;
        private float _horizontalBreadth, _verticalBreadth, _rawCrosshairX, _rawCrosshairY, _rampingPullValueDefault;
        private bool _isGrappling = false;
        [NonSerialized]public bool isGrappleHeld = true;
        private Vector3 _defaultHingePosition;
        private Vector2 _distanceFromTetherToHinge;
        private LineRenderer _grappleTetherLineRenderer;
        private Transform _grappleTetherTransform;
        private float _maxTetherDistance = 0, _newTetherDistance;
        void Start()
        {
            _grappleTetherLineRenderer = _grappleTether.GetComponent<LineRenderer>();
            _grappleTetherTransform = _grappleTether.GetComponent<Transform>();
            _grappleHingeRigidbody = _grappleHinge.GetComponent<Rigidbody2D>();

            _defaultHingePosition = _grappleHingeRigidbody.transform.localPosition;

            _screenHeight = Screen.height;
            _screenWidth = Screen.width;

            _horizontalBreadth = Camera.main.orthographicSize * _screenWidth / _screenHeight;
            _verticalBreadth = Camera.main.orthographicSize;
            Cursor.visible = false;
            _grappleHingeRigidbody.gravityScale = 0;
            _rampingPullValueDefault = _rampingPullValue;
        
        }
        void FixedUpdate()
        {
            if (_isGrappling) {
    

                /**if (maxTetherDistance > 0) {
                    newTetherDistance = ((float) Math.Sqrt((distX * distX) + (distY * distY)));
                    if (newTetherDistance < maxTetherDistance) {
                        maxTetherDistance = newTetherDistance;
                    }
                } else {
                    maxTetherDistance = ((float) Math.Sqrt((distX * distX) + (distY * distY)));
                }

                //double theta = Math.Abs(Math.Atan(distY/distX));
                //distX = (float) (distX < 0 ? Math.Cos(theta) : -1 * Math.Cos(theta));
                //distY = (float) (distY < 0 ? Math.Sin(theta) : -1 * Math.Sin(theta));

                _playerRigidbody.AddForce(_distanceFromTetherToHinge * _rampingPullValue * -1);

                _rampingPullValue += _rampingPullIncrement;
                //print(maxTetherDistance);
            } else {
                _rampingPullValue = _rampingPullValueDefault;
                _maxTetherDistance = 0;
            }

            if (!isGrappleHeld) {
                UpdateTether();
            }
        }

        void Update()
        {
            
            if (Input.GetMouseButtonDown(0)) {
                if (!isGrappleHeld) RetractGrapple();
                else LaunchGrapple();
            }
            //MoveCrosshairToCursor();
            //+print(rawCrosshairX + " " + rawCrosshairY);
        }

        /**private void MoveCrosshairToCursor() {
            Vector3 rawMousePos = Input.mousePosition;
            //(0,0) is the center of the camera
            rawCrosshairX = ((rawMousePos.x * 2f / screenWidth) - 1);
            rawCrosshairY = ((rawMousePos.y * 2f / screenHeight) - 1);

            crosshairTransform.position = new Vector3((rawCrosshairX * horizontalBreadth) + Camera.main.transform.position.x, 
                (rawCrosshairY * verticalBreadth) + Camera.main.transform.position.y, 0);
        }

        private void LaunchGrapple() {
            isGrappleHeld = false;
            _grappleHingeRigidbody.isKinematic = false;
            _grappleHingeRigidbody.transform.SetParent(null);
            _grappleHingeRigidbody.AddForce(_grappleHookTravelSpeedModifier * _hookShotAngle);
            UpdateTether();
            _grappleTetherLineRenderer.enabled = true;
        }
        public void RetractGrapple() {
            isGrappleHeld = true;
            _grappleHingeRigidbody.velocity = Vector2.zero;
            _grappleHingeRigidbody.transform.SetParent(_playerRigidbody.gameObject.transform);
            _grappleHingeRigidbody.transform.localPosition = _defaultHingePosition;
            _grappleTetherLineRenderer.enabled = false;
            _isGrappling = false;
        }

        public void PullPlayerTo(Vector2 v) {
            _isGrappling = true;
            _distanceFromTetherToHinge = new Vector2(_grappleTetherTransform.position.x - v.x , _grappleTetherTransform.position.y - v.y);
            UpdateTether();
        }

        public void PullPlayerTo(Vector3 v) {
            PullPlayerTo(new Vector2(v.x, v.y));
        }

        private void UpdateTether() {
            Vector3[] positions = new Vector3[2];
            positions[0] = _grappleTetherTransform.position;
            positions[1] = _grappleHingeRigidbody.transform.position;
            _grappleTetherLineRenderer.SetPositions(positions);
        }
        
        private Vector2 _hookShotAngle => new Vector2(1,1);
        */
    }
}