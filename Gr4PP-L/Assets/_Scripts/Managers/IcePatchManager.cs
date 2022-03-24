using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class IcePatchManager : MonoBehaviour
{
   
    //public float friction = 1f;
    private Rigidbody2D rigBody;
    private bool collide = false;
    //private bool slide = false;
    private float directionMove = 0;
    private float directionInput = 0;
    private float startSlip = 0f;
    private float endSlip = 1f;
    public float slip = 0;
    private float x = 0;
    private float y = 0;
    private Vector2 constVelocity;

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

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collide = false;
        }
    }
    
    
    void Update()
    {
        if (collide)
        {
            Debug.Log(rigBody.velocity.x);
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                rigBody.velocity = constVelocity;
                slip = startSlip;
            }
            else
            {
                directionInput = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
                directionMove = Mathf.Sign(rigBody.velocity.x);

                if (directionInput != directionMove)
                {
                    rigBody.AddForce(new Vector2(rigBody.velocity.x, rigBody.velocity.y).normalized * 0.7373f, ForceMode2D.Impulse);
                }

                //rigBody.AddForce(new Vector2(rigBody.velocity.x * -1, rigBody.velocity.y).normalized * 2f, ForceMode2D.Impulse);

                /*
                if (directionInput == directionMove)
                {
                    //rigBody.AddForce(new Vector2(rigBody.velocity.x, rigBody.velocity.y).normalized, ForceMode2D.Impulse);
                }
                else
                {
                    /*
                    rigBody.AddForce(new Vector2(rigBody.velocity.x * -1, rigBody.velocity.y).normalized * slip, ForceMode2D.Impulse);
                    if (slip > endSlip)
                    {
                        slip = slip + 0.00000000000000001f;
                    }
                    */
                /*
                if (rigBody.velocity.x < 0.01 || rigBody.velocity.x > -0.01)
                {
                    slip = 0;
                }
                */

                constVelocity = rigBody.velocity;

                /*
                directionOld = directionNew;
                directionNew = Mathf.Sign(Input.GetAxisRaw("Horizontal"));
                if (directionNew == directionOld)
                {
                    if (slip < endSlip)
                    {
                        slip = slip - 0.1f;
                    }
                }
                else
                {

                    slip = startSlip;
                }
                if (directionNew > 0)
                {
                    rigBody.AddForce(Vector2.left * slip, ForceMode2D.Impulse);
                }
                else
                {
                    rigBody.AddForce(Vector2.right * slip, ForceMode2D.Impulse);
                }
                
                */
            }
            
        }
       
         
    }
    
    
}