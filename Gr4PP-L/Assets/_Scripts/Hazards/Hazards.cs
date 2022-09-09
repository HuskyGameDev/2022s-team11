using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Restart;

namespace Hazard
{
    public class Hazards : MonoBehaviour
    {
        public Transform Spawn;   // Spawn/Beginning of level
        public GameObject player; //Our friendly robotic MC

        void OnTriggerEnter2D(Collider2D collision)  //on collision will set the player back to spawn
        { 
            player.transform.position = Spawn.transform.position;
            
        }
    }
}


