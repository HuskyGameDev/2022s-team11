using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;

    [SerializeField] private float speed = 2f;

    private float pauseTime = 0;
    private float pausedTime = 0;
    private float biasTime = 0.2f;

    [SerializeField] private bool startActive = true;
    private bool active;
    private bool finished;

    private void Start() {
        active = startActive;
        finished = false;
    }

    private void Update() {
        if (active) { 
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.01f) {
                pauseTime = waypoints[currentWaypointIndex].GetComponent<WaypointPause>().GetPause() + Time.time;
                pausedTime = Time.time;
                currentWaypointIndex++;
                if(currentWaypointIndex >= waypoints.Length) {
                    if (startActive) {
                        currentWaypointIndex = 0;
                    } else {
                        active = false;
                        finished = true;
                        Debug.Log("deactivated");
                    }
                }

            }
            if(pauseTime < Time.time){ transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed); }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && finished == false) {
            active = true;
        }
    }

    public bool GetPaused() {
        return pauseTime >= Time.time;
    }

    public bool GetBiased() {
        return pausedTime + biasTime >= Time.time;
    }

    public float GetSpeed() {
        return speed;
    }

    public GameObject[] GetWaypoints() {
        return waypoints;
    }

    public int GetCurrentIndex() {
        return currentWaypointIndex;
    }

    public bool GetActive() {
        return active;
    }
}
