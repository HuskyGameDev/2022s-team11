using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class JumpPadController : MonoBehaviour
{
    // Variable to control the launch force of the 
    [SerializeField]
    private float launchForce;

    // On collision with the player, handle the bounce direction
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D player = collision.gameObject.GetComponent<Rigidbody2D>();
            foreach (ContactPoint2D hitPos in collision.contacts)
            {
                // Gets the data from the grapplestate script and sets the isrefreshed value to true
                PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
                playerScript.CanGrapple = true;

                // Debug.Log(hitPos.normal);

                GameManager.Instance.Get<Managers.AudioManager>().Play("Jump Pad");
                // Handling the direction of the bounce
                if (hitPos.normal.y < -0.1) // from top
                {
                    //player.velocity *= Vector2.right;
                    player.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
                    Debug.Log("top");
                    return;
                }
                else if (hitPos.normal.y > 0.1) // from bottom
                {
                    //player.velocity *= Vector2.right;
                    player.AddForce(Vector2.down * launchForce, ForceMode2D.Impulse);
                    Debug.Log("bottom");
                    return;
                }
                else if (hitPos.normal.x > 0.1) // from left
                {
                    //player.velocity *= Vector2.up;
                    player.AddForce(Vector2.left * launchForce, ForceMode2D.Impulse);
                    Debug.Log("left");
                    return;
                }
                else if (hitPos.normal.x < -0.1) // from right
                {
                    //player.velocity *= Vector2.up;
                    player.AddForce(Vector2.right * launchForce, ForceMode2D.Impulse);
                    Debug.Log("right");
                    return;
                }
            }
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        }
    }
}
