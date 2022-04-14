using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using _Scripts.Movement.States;
using _Scripts.Movement;
using _Scripts.Utility;
namespace _Scripts.Managers { 

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
            playContainer = GameObject.Find("Player Container");

            // If connected with player
            if (collision.tag == "Player")
            {
                // Gets the data from the grapplestate script and sets the isrefreshed value to true
                PlayerManager playerScript = playContainer.GetComponent<PlayerManager>();
                playerScript._canGrapple = true;
                Destroy(gameObject);
            }
        }
    }
}
