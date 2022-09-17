using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using Movement;
using Utility;

namespace Managers { 

    /** Author: Josh Robinson
        * Version 2/7/22
        */

    public class RefreshManager : MonoBehaviour
    {
        // Container to hold the player data
        public GameObject playContainer;
    
        // When player collides with refresher
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Player container location
            playContainer = GameObject.FindGameObjectWithTag("Player");

            // If connected with player
            if (collision.tag == "Player")
            {
                // Gets the data from the grapplestate script and sets the isrefreshed value to true
                PlayerController playerScript = playContainer.GetComponent<PlayerController>();
                playerScript.CanGrapple = true;
                Destroy(gameObject);
            }
        }
    }
}
