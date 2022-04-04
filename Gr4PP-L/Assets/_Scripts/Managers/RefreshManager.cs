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
        public GameObject playContainer;
    

        private void OnTriggerEnter2D(Collider2D collision)
        {
            playContainer = GameObject.Find("Player Container");
            if (collision.tag == "Player")
            {
                PlayerManager playerScript = playContainer.GetComponent<PlayerManager>();
                playerScript._grapplingState.isRefreshed = true;
                Destroy(gameObject);
            }
        }
    }
}
