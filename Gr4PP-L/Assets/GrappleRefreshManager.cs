using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Author: Josh Robinson
    * Version 2/12/22
    */
public class GrappleRefreshManager : MonoBehaviour
{
    SpriteRenderer sprite;
    public bool GrappleActive = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GrappleActive == true)
        {
            sprite.color = new Color(0, 1, 0, 1);
        } else
        {
            sprite.color = new Color(1, 1, 1, 1);
        }
    }
}
