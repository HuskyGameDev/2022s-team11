using UnityEngine;
namespace _Scripts.Movement {
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

        void OnTriggerEnter2D(Collider2D c)
        {
            if (_isHeld) return;

            if (!c.gameObject.CompareTag("Player")) {
                AttachHookTo(c.gameObject);
            } else if (_isAttached) {
                RetractHook();
            }
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
            
            _rb.isKinematic = false;
            _rb.velocity = direction.normalized * force;
            _isHeld = false;
            _lr.enabled = true;
        }

    }
}