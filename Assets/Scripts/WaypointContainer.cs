using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public List<Transform> waypointsToTarget = new List<Transform>();
    public float distanceTotal = 0;
    public float distanceBisDrohnenshot = 0;
    private Transform prev = null;
    private float distance;

    void Awake()
    {
        // Initialize and add all child transforms except the parent itself
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>())
        {
            waypoints.Add(t);
        }
        if (waypoints.Count > 0)
        {
            waypoints.RemoveAt(0); // Remove the parent transform
        }
    }

    public float GetDistanceTotal()
    {
        // Reset previous transform and total distance
        prev = null;
        distanceTotal = 0;

        for (int i = 0; i < waypoints.Count; i++)
        {
            if (prev == null)
            {
                prev = waypoints[i];
            }
            else
            {
                distance = Vector3.Distance(prev.position, waypoints[i].position);
                distanceTotal += distance;
                prev = waypoints[i];
            }
        }
        return distanceTotal;
    }

    public float GetDistanceBisDrohnenshot(List<Transform> list)
    {
        // Reset previous transform and partial distance
        prev = null;
        distanceBisDrohnenshot = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (prev == null)
            {
                prev = list[i];
            }
            else
            {
                distance = Vector3.Distance(prev.position, list[i].position);
                distanceBisDrohnenshot += distance;
                prev = list[i];
                //Debug.Log("One distance: " + distance);
            }
        }

        

        return distanceBisDrohnenshot;
    }

    public List<Transform> GetWaypointUpToIndex(int index)
    {
        List<Transform> list = new List<Transform>();
        for (int i = 0; i <= index && i < waypoints.Count; i++)
        {
            list.Add(waypoints[i]);
        }
        return list;
    }

    
}



