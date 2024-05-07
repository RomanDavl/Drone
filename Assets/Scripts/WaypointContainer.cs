using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> waypoints;

    void Awake()
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            waypoints.Add(t);
        }
        waypoints.Remove(waypoints[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
