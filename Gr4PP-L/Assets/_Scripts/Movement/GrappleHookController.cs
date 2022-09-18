using UnityEngine;
namespace Movement {
    public class GrappleHookController : MonoBehaviour
    {
        private bool _isHeld, _isAttached;
        public bool IsAttached => _isAttached;
        public bool IsHeld => _isHeld;
        private Vector2 _playerHoldPosition;
        public Vector2 Position => (Vector2) this.transform.position;
        public Vector2 TetherPosition => (Vector2) _grappleTether.position;
        [SerializeField] private GameObject _parent;
        [SerializeField] private Transform _grappleTether;
        [SerializeField] private Vector2 _defaultDirectionVector;
        [SerializeField] private LayerMask _validGrappleLayers;
        private Rigidbody2D _rb;
        private LineRenderer _lr;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _lr = GetComponent<LineRenderer>();
            _rb.gravityScale = 0;
            _rb.isKinematic = true;
            _isHeld = true;
            _isAttached = false;
            _playerHoldPosition = this.transform.localPosition;
        }

        void Update()
        {
            if (!_isAttached) return;

            _lr.SetPosition(0, this.Position);
            _lr.SetPosition(1, _grappleTether.position);  
        }

        /*
        void OnTriggerEnter2D(Collider2D c)
        {
            if (_isHeld) return;

            if (!c.gameObject.CompareTag("Player")) {
                AttachHookTo(c.gameObject);
            } else if (_isAttached) {
                RetractHook();
            }
        }
        */

        void OnTriggerEnter2D(Collider2D c) {
            if (_isHeld) return;

            if (c.gameObject.CompareTag("Player") && _isAttached) RetractHook();
        }

        /// <summary>
        /// Attaches this grappling hook as a child object of a specified game object.
        /// </summary>
        /// <param name="g">The GameObject to attach the hook to</param>
        private void AttachHookTo(GameObject g) {
            MoveHookTo(g);
            _isAttached = true;
            _isHeld = false;
        }

        private void MoveHookTo(GameObject g) {
            this.transform.SetParent(g.transform);
            _rb.velocity = Vector3.zero;
            _rb.isKinematic = true;
        }

        public void RetractHook() {
            MoveHookTo(_parent);
            SetLocalPosition(_playerHoldPosition);
            _isAttached = false;
            _isHeld = true;
            _lr.enabled = false;
        }

        public void SetLocalPosition(Vector2 pos) {
            this.transform.localPosition = pos;
        }

        public void FireHook(Vector2 direction, float force) {
            if (direction.x == 0 && direction.y == 0) direction = _defaultDirectionVector;

            //RaycastHit2D _hit = Physics2D.Raycast(_parent.transform.position + new Vector3(0, 0.75f, 0), direction, 15, _validGrappleLayers);
            RaycastHit2D _hit = castHit(ref direction, 15, _validGrappleLayers);
            //Debug.DrawRay(this.transform.position, direction, Color.red);

            if (_hit) {
                Vector2 _locationDelta = _hit.distance * direction.normalized;
                this.transform.Translate(_locationDelta);
                _isHeld = false;
                _lr.enabled = true;
                AttachHookTo(_hit.collider.gameObject);
            }

            //_rb.isKinematic = false;
            //_rb.velocity = direction.normalized * force;
        }

        private RaycastHit2D castHit(ref Vector2 direction, float distance, LayerMask layerMask) {;

            RaycastHit2D hit = Physics2D.Raycast(_parent.transform.position + new Vector3(0, 0.75f, 0), direction, distance, layerMask);
            if (hit && !hit.collider.gameObject.CompareTag("Ice") && !hit.collider.gameObject.CompareTag("Jump Pad") && !hit.collider.gameObject.CompareTag("Glass")) return hit;

            int width = 30;
            float fidelity = 1;
            if(direction.normalized.y == 0 || direction.normalized.x == 0) {
                width = 0;
            }

            for(float i = 1/fidelity; i <= width; i += 1/fidelity) {
                hit = Physics2D.Raycast(_parent.transform.position + new Vector3(0, 0.75f, 0), getVectorFromAngle(Vector2.SignedAngle(Vector2.right, direction) + i), distance, layerMask);
                if (hit && !hit.collider.gameObject.CompareTag("Ice") && !hit.collider.gameObject.CompareTag("Jump Pad") && !hit.collider.gameObject.CompareTag("Glass")) {
                    direction = getVectorFromAngle(Vector2.SignedAngle(Vector2.right, direction) + i);
                    return hit;
                }
                hit = Physics2D.Raycast(_parent.transform.position + new Vector3(0, 0.75f, 0), getVectorFromAngle(Vector2.SignedAngle(Vector2.right, direction) - i), distance, layerMask);
                if (hit && !hit.collider.gameObject.CompareTag("Ice") && !hit.collider.gameObject.CompareTag("Jump Pad") && !hit.collider.gameObject.CompareTag("Glass")) {
                    direction = getVectorFromAngle(Vector2.SignedAngle(Vector2.right, direction) - i);
                    return hit;
                }
            }
            return Physics2D.Raycast(_parent.transform.position + new Vector3(0, 0.75f, 0), direction, 0, layerMask);
        }

        /* gets a vector from a given angle
         * 
         * parameter angle: angle to get the vector from
         * return: Vector3 equivalent to the angle given
         */
        public static Vector2 getVectorFromAngle(float angle) {
            float angleRad = angle * (Mathf.PI / 180f);
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }
    }
}