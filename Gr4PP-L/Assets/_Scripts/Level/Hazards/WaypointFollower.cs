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

    private void Update() {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.01f) {
            pauseTime = waypoints[currentWaypointIndex].GetComponent<WaypointPause>().GetPause() + Time.time;
            pausedTime = Time.time;
            currentWaypointIndex++;
            if(currentWaypointIndex >= waypoints.Length) {
                currentWaypointIndex = 0;
            }

        }
        if(pauseTime < Time.time){ transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed); }
        
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
}
