using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class AICarController : MonoBehaviour
{
    [SerializeField] private WaypointContainer waypointContainer;
    private List<Transform> waypoints;
    private int currentWaypoint;
    private CarController carController;
    private float waypointRange;
    private float currentAngle;
    [SerializeField] private float gasInput;
    [SerializeField] private bool isInsideBraking;
    public bool IsInsideBraking { get { return isInsideBraking; } set { isInsideBraking = value; } }
    private float maxAngle = 40f;
    private float maxSpeed = 300f;

    void Start()
    {
        carController = GetComponent<CarController>();
        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0;
        waypointRange = 2f;
    }

    void Update()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) < waypointRange)
        {
            currentWaypoint++;
            if (currentWaypoint == waypoints.Count)
                currentWaypoint = 0;
        }

        Vector3 forward = transform.TransformDirection(Vector3.back);

        Vector3 waypointDirection = (waypoints[currentWaypoint].position - transform.position).normalized;
        currentAngle = Vector3.SignedAngle(forward, waypointDirection, Vector3.up);

        gasInput = Mathf.Clamp01(maxAngle - Mathf.Abs(carController.Speed * 0.01f * currentAngle) / maxAngle);
        if (isInsideBraking)
        {
            gasInput = -gasInput * (Mathf.Clamp01((carController.Speed / maxSpeed) * 2 - 1f));
        }
        carController.SetInput(gasInput, currentAngle);

        Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.yellow);
    }
}
