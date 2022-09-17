using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class IcePatchManager : MonoBehaviour
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

    private void Start() {
        _gm = GameManager.Instance;
    }
    
    // Handles the sliding variables when the player comes in contact with the object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _rigBody = collision.gameObject.GetComponent<Rigidbody2D>();
            _constVelocity = _rigBody.velocity;
            _collide = true;
            slip = _startSlip;
        }
    }

    // Handles a collision exit gracefully
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collide = false;
        }
    }
    
    // Update function
    void Update()
    {
        if (_collide)
        {
            // Gets the players input and their direction
            _directionInput = _gm.DirectionalInput.x;
            _directionMove = _rigBody.velocity.x;
            // Debug.Log(_directionMove);

            // Controls players movement when not pressing anything
            if (_directionInput == 0)
            {
                _rigBody.velocity = new Vector2 (_constVelocity.x, _rigBody.velocity.y);
                slip = 0f;
            } 
            else
            {              
                // Controls players rightward movement
                if (_directionInput > 0)
                {
                    // Checking for little to no movement
                    if (_directionMove < 0.009f && _directionMove > -0.009f)
                    {
                        // Debug.Log("RIGHT");
                        _rigBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
                    } 
                    else
                    {
                        slip = slip * _directionMove;
                        _rigBody.AddForce(Vector2.right * -slip, ForceMode2D.Impulse);
                        _constVelocity = _rigBody.velocity;
                    }
                }
                // Controls players leftward movement
                else if (_directionInput < 0)
                {
                    if (_directionMove < 0.009f && _directionMove > -0.009f)
                    {
                        // Debug.Log("LEFT");
                        _rigBody.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);
                    }
                    else
                    {
                        slip = slip * _directionMove;
                        _rigBody.AddForce(Vector2.left * -slip, ForceMode2D.Impulse);
                        _constVelocity = _rigBody.velocity;
                    }  
                } 
            }
        }
       
         
    }
    
    
}