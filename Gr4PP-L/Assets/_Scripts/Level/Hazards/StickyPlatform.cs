using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Level;

namespace Level {
    public class StickyPlatform : MonoBehaviour {
        public WaypointFollower wf;

        private void Awake() {
            wf = GetComponent<WaypointFollower>();
        }

        //works pretty well, but buffered jumps can mess it up. also the camera doesn't follow it very well, ends up being jittery.

        private void OnTriggerEnter2D(Collider2D collision) {
            Debug.Log("Trigger Entered");
            if (collision.gameObject.CompareTag("Player")) {
                if (!wf.GetActive()) {
                    collision.gameObject.transform.SetParent(transform);
                    return;
                }

                collision.gameObject.transform.SetParent(transform);

                GameObject[] waypoints = wf.GetWaypoints();
                int currentWaypointIndex = wf.GetCurrentIndex();
                float speed = wf.GetSpeed();

                if (!wf.GetPaused()) {
                    Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                    Vector2 platformVector = waypoints[currentWaypointIndex].transform.position - waypoints[Mathf.Abs((currentWaypointIndex - 1) % waypoints.Length)].transform.position;
                    Vector2 platformVelocity = platformVector.normalized * speed;

                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(playerVelocity.x - platformVelocity.x, playerVelocity.y);
                } else if (wf.GetBiased()) {
                    Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                    Vector2 platformVector = waypoints[Mathf.Abs((currentWaypointIndex - 1) % waypoints.Length)].transform.position - waypoints[Mathf.Abs((currentWaypointIndex - 2) % waypoints.Length)].transform.position;
                    Vector2 platformVelocity = platformVector.normalized * speed;

                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(playerVelocity.x - platformVelocity.x, playerVelocity.y);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            
            if (collision.gameObject.CompareTag("Player")) {
                collision.gameObject.transform.SetParent(null);
                if (!wf.GetActive()) {
                    // platforms was not active when the player exited the trigger.
                    return;
                }

                GameObject[] waypoints = wf.GetWaypoints();
                int currentWaypointIndex = wf.GetCurrentIndex();
                float speed = wf.GetSpeed();

                if (!wf.GetPaused()) {
                    Debug.Log("waypoints.Length: " + waypoints.Length);
                    Debug.Log("Given index: " + Mathf.Abs((currentWaypointIndex - 1) % waypoints.Length));
                    Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                    Vector2 platformVector = waypoints[currentWaypointIndex].transform.position - waypoints[Mathf.Abs((currentWaypointIndex - 1) % waypoints.Length)].transform.position;
                    Vector2 platformVelocity = platformVector.normalized * speed;

                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = playerVelocity + platformVelocity;

                    Debug.Log("Player Velocity: " + playerVelocity);
                    Debug.Log("Platform Velocity: " + platformVelocity);
                } else if (wf.GetBiased()) {
                    Vector2 playerVelocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
                    Vector2 platformVector = waypoints[Mathf.Abs((currentWaypointIndex - 1) % waypoints.Length)].transform.position - waypoints[Mathf.Abs((currentWaypointIndex - 2) % waypoints.Length)].transform.position;
                    Vector2 platformVelocity = platformVector.normalized * speed;

                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = playerVelocity + platformVelocity;

                    Debug.Log("Player Velocity: " + playerVelocity);
                    Debug.Log("Platform Velocity: " + platformVelocity);
                }
            }
        }
    }
}
