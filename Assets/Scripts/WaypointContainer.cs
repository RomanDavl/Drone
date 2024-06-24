using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> waypoints;
    public float distanceTotal = 0;
    private Transform prev=null;
    private float distance;

    void Awake()
    {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            

            waypoints.Add(t);
        }
        waypoints.Remove(waypoints[0]);
       
    }

    public float GetDistanceTotal ()
    {
        foreach (Transform t in waypoints)
        {
            if (prev == null)
            {
                prev = t;

            }
            else
            {
                distance = Vector3.Distance(prev.transform.position, t.transform.position);
                //Debug.Log(distance);
                distanceTotal += distance;
                prev = t;
            }
        }
        return distanceTotal;
    }

   
}
