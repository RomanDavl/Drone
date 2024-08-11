using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
        if (Vector3.Distance(waypoints[currentWaypoint].position, transform.position) < waypointRange)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Count)
                currentWaypoint = waypoints.Count - 1;
                return;
            
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        currentAngle = Vector3.SignedAngle(forward, waypoints[currentWaypoint].position - transform.position, Vector3.up);

        gasInput = Mathf.Clamp01(maxAngle - Mathf.Abs(carController.Speed * 0.02f * currentAngle) / maxAngle);

        carController.MaxSpeed = isInsideBraking ? 40f : 80f;
        

        carController.SetInput(gasInput, currentAngle);

        if (carController.Speed > carController.MaxSpeed)
        {
            carController.Speed = carController.MaxSpeed;
        }


        //carController.SetInput(gasInput, currentAngle);

        //Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.yellow);
    }

    
}
