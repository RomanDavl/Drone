using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManagaer : MonoBehaviour
{
     public DroneMovement drone;

    public void SetWaypoint(Transform newWaypoint)
    {
        drone.targetWaypoint = newWaypoint;
    }
}
