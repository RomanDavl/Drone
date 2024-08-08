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

    //gesamte Strecke von Start bis Ende des Autos
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

    // Strecke des Autos bis die Drohne das Auto trifft
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

    // alle Waypoints bis zum übergebenen Waypoint 
    public List<Transform> GetAllWaypointsUpToWaypoint(Transform waypoint)
    {
        List<Transform> list = new List<Transform>();

        Boolean atWaypoint = false;


        foreach (Transform t in waypoints)
        {
            if (atWaypoint== false)
            {
                if (t == waypoint)
                {
                    list.Add(t);
                    atWaypoint = true;
                }
                else
                {
                    list.Add(t);
                }
            }
            else
            {
                break;
            }        
                
        }
        
        return list;
    }

    // alle Waypoints ab dem übergebenen Waypoint 
    public List<Transform> GetAllWaypointsStartingAtWaypoint(Transform waypoint)
    {
        List<Transform> list = new List<Transform>();

        Boolean atWaypoint = false;


        foreach (Transform t in waypoints)
        {
            if (atWaypoint == false)
            {
                if (t == waypoint)
                {
                    atWaypoint = true;
                    list.Add(t);
                }
                else
                {
                    continue;
                }
            }
            else
            {
                list.Add(t);
            }

        }

        return list;
    }

}



