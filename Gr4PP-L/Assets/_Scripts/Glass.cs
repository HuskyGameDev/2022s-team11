using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Glass : MonoBehaviour {
    public Tilemap _glassTilemap;
    public TilemapCollider2D _glassCollider;
    public GameObject _character;
    
    [SerializeField]
    [Tooltip("How fast the player must be moving to break the glass")]
    private float _threshold;
    [SerializeField]
    [Tooltip("The direction the player will be breaking through the glass from")]
    private bool _isHorizontal;

    private void Start() {
        _glassTilemap = GetComponent<Tilemap>();
        _glassCollider = GetComponent<TilemapCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("x: " + collision.relativeVelocity.x);
        Debug.Log("y: " + collision.relativeVelocity.y);
        if (_isHorizontal) {
            if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.x) > _threshold) {
                Debug.Log("Shattered");
                _character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity; 
                gameObject.SetActive(false);
            }
        } else {
            if (collision.gameObject.CompareTag("Player") && Mathf.Abs(collision.relativeVelocity.y) > _threshold) {
                Debug.Log("Shattered");
                _character.GetComponent<Rigidbody2D>().velocity = collision.relativeVelocity;
                gameObject.SetActive(false);
            }
        }
    }
    /*
    private void FixedUpdate() {
        if (_isHorizontal) {
            if (Mathf.Abs(_prb.velocity.x) > _threshold) {
                _glassCollider.isTrigger = true;
            } else {
                _glassCollider.isTrigger = false;
            }
        } else {
            if (Mathf.Abs(_prb.velocity.y) > _threshold) {
                _glassCollider.isTrigger = true;
            } else {
                _glassCollider.isTrigger = false;
            }
        }
    }
    */
}
