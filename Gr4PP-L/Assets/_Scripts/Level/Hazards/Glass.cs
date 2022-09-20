using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Glass : MonoBehaviour {
    public GameObject _character; // specifically the Character GameObject, not Player Controller.
    
    [SerializeField]
    [Tooltip("How fast the player must be moving to break the glass")]
    private float _threshold;
    [SerializeField]
    [Tooltip("The direction the player will be breaking through the glass from")]
    private bool _isHorizontal;

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.relativeVelocity);
        if (_isHorizontal) {
            if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.x) > _threshold) {
                _character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                gameObject.SetActive(false);
            }
        } else {
            if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.y) > _threshold) {
                _character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; // ensures player doesn't lose velocity on contact
                gameObject.SetActive(false);
            }
        }
    }
}
