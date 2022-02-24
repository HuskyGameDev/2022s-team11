using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/23/22
    */
public class GrappleMarkerScript : MonoBehaviour
{
    SpriteRenderer sprite;
    private bool grappleReady = true;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //
        sprite.color = new Color(0, 1, 0, 1);
        if (grappleReady == true)
        {
            sprite.color = new Color(0, 1, 0, 1);
        }
        else
        {
            sprite.color = new Color(1, 0, 0, 1);
        }
    }
}
