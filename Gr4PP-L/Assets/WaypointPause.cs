using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPause : MonoBehaviour
{
    [SerializeField] private float pause;

    public float GetPause() {
        return pause;
    }
}
