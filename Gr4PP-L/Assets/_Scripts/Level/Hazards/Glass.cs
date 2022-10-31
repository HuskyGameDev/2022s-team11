using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level {
    public class Glass : Respawnable {
        
        [SerializeField]
        [Tooltip("How fast the player must be moving to break the glass")]
        private float _threshold;
        [SerializeField]
        [Tooltip("The direction the player will be breaking through the glass from")]
        private bool _isHorizontal;

        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.transform.tag != "Player") return;
            GameObject character = collision.gameObject;

            if (_isHorizontal) {
                if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.x) > _threshold) {
                    character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                    Deactivate();
                }
            } else {
                if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.y) > _threshold) {
                    character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                    Deactivate();
                }
            }
        }
    }
}