using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;

public class GPS : MonoBehaviour
{
    [SerializeField] private WaypointContainer waypointContainer;
    [SerializeField] private GameObject car;
    private AICarController controller;
    private List<Transform> waypoints;
    private LineRenderer lineRenderer;
    private int currentWaypoint;

    void Start()
    {   
        controller = car.GetComponent<AICarController>();
        waypoints = controller.waypoints;
        lineRenderer = GetComponent<LineRenderer>();  
    }


    // Update is called once per frame
    void Update()
    {
        currentWaypoint = controller.currentWaypoint;
        UpdatePath();
    }

    private void UpdatePath()
    {
        int remainingWaypoints = waypoints.Count - currentWaypoint;
        lineRenderer.positionCount = remainingWaypoints + 1;

        Vector3 carMinimapPosition = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        lineRenderer.SetPosition(0, carMinimapPosition);

        for (int i = 0; i < remainingWaypoints; i++)
        {
            Vector3 wpPosition = waypoints[currentWaypoint + i].position;
            wpPosition.y = 19;
            lineRenderer.SetPosition(i + 1, wpPosition);
        }
    }

}
