using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class IcePatchManager : MonoBehaviour
{
    // Varaibles to control sliding
    private Rigidbody2D rigBody;
    private bool collide = false;
    private float directionMove = 0f;
    private float directionInput = 0f;
    private float startSlip = 0f;
    public float slip = 0;
    private Vector2 constVelocity;
    
    // Handles the sliding variables when the player comes in contact with the object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rigBody = collision.gameObject.GetComponent<Rigidbody2D>();
            constVelocity = rigBody.velocity;
            collide = true;
            slip = startSlip;
        }
    }

    // Handles a collision exit gracefully
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collide = false;
        }
    }
    
    // Update function
    void Update()
    {
        if (collide)
        {
            // Gets the players input and their direction
            directionInput = Input.GetAxisRaw("Horizontal");
            directionMove = rigBody.velocity.x;
            // Debug.Log(directionMove);

            // Controls players movement when not pressing anything
            if (directionInput == 0)
            {
                rigBody.velocity = constVelocity;
                slip = 0f;
            } 
            else
            {              
                // Controls players rightward movement
                if (directionInput > 0)
                {
                    // Checking for little to no movement
                    if (directionMove < 0.009f && directionMove > -0.009f)
                    {
                        // Debug.Log("RIGHT");
                        rigBody.AddForce(Vector2.right * 0.1f, ForceMode2D.Impulse);
                    } 
                    else
                    {
                        slip = slip * directionMove;
                        rigBody.AddForce(Vector2.right * -slip, ForceMode2D.Impulse);
                        constVelocity = rigBody.velocity;
                    }
                }
                // Controls players leftward movement
                else if (directionInput < 0)
                {
                    if (directionMove < 0.009f && directionMove > -0.009f)
                    {
                        // Debug.Log("LEFT");
                        rigBody.AddForce(Vector2.left * 0.1f, ForceMode2D.Impulse);
                    }
                    else
                    {
                        slip = slip * directionMove;
                        rigBody.AddForce(Vector2.left * -slip, ForceMode2D.Impulse);
                        constVelocity = rigBody.velocity;
                    }  
                } 
            }
        }
       
         
    }
    
    
}