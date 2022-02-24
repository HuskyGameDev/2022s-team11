using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class IcePatchManager : MonoBehaviour
{
    public float slipFactor = 0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            slipFactor = collision.gameObject.GetComponent<Rigidbody2D>().velocity.x;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * slipFactor, ForceMode2D.Impulse);
        }
    }
}
