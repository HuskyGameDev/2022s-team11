using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/21/22
    */
public class GrappleActiveScript : MonoBehaviour
{
    SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = new Color(0, 1, 0, 1);
        /**
        if (Player Character.GrappleReady)
        {
            sprite.color = new Color(0, 1, 0, 1);
        } 
        else
        {
            sprite.color = new Color(1, 0, 0, 1);
        }
        */
    }
}
