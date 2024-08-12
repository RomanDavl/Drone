using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class AICarController : MonoBehaviour
{
    [SerializeField] private WaypointContainer waypointContainer;

    public List<Transform> waypoints;
    public int currentWaypoint;
    private CarController carController;
    private float waypointRange;
    private float currentAngle;
    [SerializeField] private float gasInput;
    [SerializeField] private bool isInsideBraking;
    public bool IsInsideBraking { get { return isInsideBraking; } set { isInsideBraking = value; } }
    private float maxAngle = 45f;

    void Start()
    {
        carController = GetComponent<CarController>();
        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0;
        waypointRange = 3f;
    }

    void Update()
    {
        if (currentWaypoint >= waypoints.Count || Vector3.Distance(waypoints[currentWaypoint].position, transform.position) < waypointRange)
        {
            if (currentWaypoint >= waypoints.Count)
            {
                gasInput = 0;
                currentAngle = 0;
                carController.SetInput(gasInput, currentAngle);
                Debug.Log("before stop" + transform.name + ": " + currentWaypoint + ", " + waypoints.Count + ", " + currentAngle);
                return;
            }

            if (currentWaypoint != waypoints.Count - 1)
            {
                currentWaypoint++;
            }
        }

        Debug.Log("after stop" + transform.name + ": " + currentWaypoint + ", " + waypoints.Count + ", " + currentAngle);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        currentAngle = Vector3.SignedAngle(forward, waypoints[currentWaypoint].position - transform.position, Vector3.up);

        gasInput = Mathf.Clamp01(maxAngle - Mathf.Abs(carController.Speed * 0.02f * currentAngle) / maxAngle);

        carController.MaxSpeed = isInsideBraking ? 40f : 80f;


        carController.SetInput(gasInput, currentAngle);

        if (carController.Speed > carController.MaxSpeed)
        {
            carController.Speed = carController.MaxSpeed;
        }

        if (carController.Speed <= 5 && gasInput > 0)
        {
            Rigidbody rb = carController.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 200f, ForceMode.Impulse);
        }

        //carController.SetInput(gasInput, currentAngle);

        Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.yellow);

        if (currentWaypoint == waypoints.Count - 1)
        {
            currentWaypoint++;
        }
    }


}
