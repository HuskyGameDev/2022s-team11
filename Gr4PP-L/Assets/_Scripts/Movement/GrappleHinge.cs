using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Movement {
    public class GrappleHinge : MonoBehaviour
    {
        [SerializeField]
        private GameObject grappleHinge;
        [SerializeField]
        private GrapplingHook grappleShooter;
        private Rigidbody2D grappleHingeRigidbody;
        private bool isAttached = false;
        // Start is called before the first frame update
        void Start()
        {
            grappleHingeRigidbody = grappleHinge.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnTriggerEnter2D(Collider2D c)
        {
            if (!c.gameObject.CompareTag("Player") && !grappleShooter.isGrappleHeld) {
                AttachHookTo(c.gameObject);
                grappleShooter.PullPlayerTo(grappleHinge.transform.position);
            } else if (isAttached) {
                grappleShooter.RetractGrapple();
                isAttached = false;
            }
        }

        private void AttachHookTo(GameObject g) {
            grappleHinge.transform.SetParent(g.transform);
            grappleHingeRigidbody.velocity = Vector3.zero;
            grappleHingeRigidbody.isKinematic = true;
            isAttached = true;
        }
    }
}