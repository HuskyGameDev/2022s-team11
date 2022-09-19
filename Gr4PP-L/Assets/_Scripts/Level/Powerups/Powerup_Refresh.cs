using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Movement;
using Utility;

namespace Level { 

    /** Author: Josh Robinson
        * Version 2/7/22
        */

    public class Powerup_Refresh : Powerup
    {   
        // When player collides with refresher
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // If connected with player
            if (collision.tag == "Player")
            {
                // Gets the data from the grapplestate script and sets the isrefreshed value to true
                PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
                playerScript.CanGrapple = true;
                
                Pickup();
            }
        }
    }
}
