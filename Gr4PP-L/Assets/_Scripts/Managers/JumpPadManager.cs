using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class JumpPadManager : MonoBehaviour
{
    public float launchForce = 100f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
        }
    }
}
