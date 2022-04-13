using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Restart
{
    public class Respawn: MonoBehaviour
    {
        public Transform spawn;   // Spawn/Beginning of level
        public GameObject Player;       // Our friendly robotic MC
        private bool touchingHazard = false;     // Are you touching the bad?

        void OnTriggerEnter2D(Collider2D collision)
        {
            touchingHazard = true;       // If touching the bad, set to true
        }

        void respawn()
        {
            /* 
             * Teleports the player back to original starting point of this level
             * MAY NEED VECTOR OFFSET IN CASE OF BGINNING STUCK IN SOMETHING, NEEDS FURTHER TESTING
             * then sets touchinghazard to false, because you are safe
             */
            Player.transform.position = spawn.transform.position;
            touchingHazard = false;
        }

        void Update()
        {
            // if player is touching the badbad
            if (touchingHazard)
            {
                respawn();                                       // Respawns the player
                Application.LoadLevel(Application.loadedLevel);  // Reloads the whole level, resetting the objects
                touchingHazard = false;    // Sets back to false (if not already)
            }
        }
    }
}