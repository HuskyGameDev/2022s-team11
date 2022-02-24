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
            if (slipFactor < 5f && slipFactor > -5f)
            {
                if (slipFactor > 0f)
                {
                    slipFactor = 5f;
                } else
                {
                    slipFactor = -5f;
                }
            }
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * slipFactor, ForceMode2D.Impulse);
        }
    }
}
