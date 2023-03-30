using Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class IcePatchController : MonoBehaviour
{
    private GameManager _gm;

    // Varaibles to control sliding
    private Rigidbody2D _rigBody;
    private bool _collide = false;
    private float _directionMove = 0f;
    private float _directionInput = 0f;
    private float _startSlip = 0f;
    public float slip = 0;
    private Vector2 _constVelocity;
    private float _frictionAmount = 0.1f;

    private void Start()
    {
        _gm = GameManager.Instance;
    }

    // Handles the sliding variables when the player comes in contact with the object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*            _rigBody = collision.gameObject.GetComponent<Rigidbody2D>();
                        _constVelocity = _rigBody.velocity;
                        _collide = true;
                        slip = _startSlip;*/

            collision.gameObject.GetComponent<PlayerController>().OnIce();
        }
    }

    // Handles a collision exit gracefully
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //_collide = false;
            collision.gameObject.GetComponent<PlayerController>().OffIce();
        }
    }

    // Update function
    /*void FixedUpdate()
    {
        if (!_collide) return;

        // Gets the players input and their direction
        _directionInput = _gm.DirectionalInput.x;
        _directionMove = _rigBody.velocity.x;

        // Controls players movement when not pressing anything
        if (_directionInput == 0)
        {
            _rigBody.velocity = new Vector2(_constVelocity.x * (1 - _frictionAmount), _rigBody.velocity.y);
            slip = 0f;
            return;
        }

        // Controls players movement

        slip = slip * _directionMove;
        //If the player tries to move away from the direction they're sliding in, reduce that movement
        _rigBody.AddForce(new Vector2(_directionMove * 0.1f, 0), ForceMode2D.Impulse);
        _constVelocity = _rigBody.velocity;
    }*/
}