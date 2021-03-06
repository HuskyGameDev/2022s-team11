using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class JumpPadManager : MonoBehaviour
{
    // Variable to control the launch force of the 
    [SerializeField]
    private float launchForce;

    // On collision with the player, handle the bounce direction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach(ContactPoint2D hitPos in collision.contacts)
            {
                // Debug.Log(hitPos.normal);
                
                // Handling the direction of the bounce
                if (hitPos.normal.y < 0) // from top
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity *= Vector2.right;
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
                }
                else if (hitPos.normal.y > 0) // from bottom
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity *= Vector2.right;
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.down * launchForce, ForceMode2D.Impulse);
                }
                else if (hitPos.normal.x > 0) // from left
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity *= Vector2.up;
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * launchForce, ForceMode2D.Impulse);
                }
                else if (hitPos.normal.x < 0) // from right
                {
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity *= Vector2.up;
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * launchForce, ForceMode2D.Impulse);
                }
            }
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        }
    }
}
