using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class DroneMovement : MonoBehaviour
{
    public Transform targetWaypoint; // Das Zielobjekt
    public float speed = 15f; // Geschwindigkeit der Drohne
    public float rotationSpeed = 2f; // Drehgeschwindigkeit der Drohne
    public float hoverHeight = 10f; // Höhe, in der die Drohne über dem Waypoint schweben soll
    public float carFollowSpeed = 20f; // Geschwindigkeit, mit der die Drohne dem Auto folgt

    private bool followCar = false; // Flag, um das Folgen des Autos zu aktivieren
    private Transform carTarget; // Das Auto-Zielobjekt
    private Transform cameraTransform; // Die Kamera, die an der Drohne befestigt ist
    
    int i = 0;

    [SerializeField] private WaypointContainer waypointContainer;
    private List<Transform> waypoints;
    private int currentWaypoint;
    private float currentAngle;
    private float waypointRange;

    private float zooom = 1;
    public float zoomSpeed = 5f;
    
    


    void Start()
    {
        // Findet die Hauptkamera, die als Kind der Drohne angehängt ist
        cameraTransform = Camera.main.transform;

        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0;
        waypointRange = 3f;
        float total = waypointContainer.GetDistanceTotal();
        Debug.Log(total);
       
    }

    void Update()
    {
        
        if (!followCar && targetWaypoint == null)
        {
            Debug.LogError("Target Waypoint is not set.");
            return;
        }

        if (followCar)
        {
            //FollowCar();
            Drohnenshot();
        }
        else
        {
            FlyToWaypoint();
        }
    }

    void FlyToWaypoint()
    {
        if (targetWaypoint != null)
        {
            // Zielposition berechnen, die 10 Meter über dem Waypoint liegt
            Vector3 targetPosition = new Vector3(targetWaypoint.position.x, targetWaypoint.position.y + hoverHeight, targetWaypoint.position.z);

            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            transform.position += direction * speed * Time.deltaTime;

            // Optional: Wenn die Drohne die Zielposition erreicht, könnte sie stoppen
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Drohne hat den Waypoint erreicht und schwebt darüber!");
                targetWaypoint = null; // Stopp die Drohne oder setze ein neues Ziel
            }
        }
    }

    void FollowCar()
    {
        if (carTarget != null)
        {
            // Zielposition berechnen, die 5 Meter hinter und 10 Meter über dem Auto liegt
            Vector3 targetPosition = carTarget.position - carTarget.forward * 5f + Vector3.up * hoverHeight;

            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            transform.position += direction * carFollowSpeed * Time.deltaTime;

            // Kamera soll stets auf das Auto gerichtet sein
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
    }

   

    void Drohnenshot()
    {
        if (currentWaypoint >= waypoints.Count)
    {
        // Wenn alle Waypoints erreicht sind, kann die Funktion beendet werden
        return;
    }

    // Berechne das Ziel basierend auf dem aktuellen Waypoint
    Vector3 target;

    if (currentWaypoint < 4)
    {
        target = new Vector3(waypoints[currentWaypoint].position.x + 50, hoverHeight, waypoints[currentWaypoint].position.z + 60);
        //zooom += zoomSpeed * Time.deltaTime;

    }
    else if (currentWaypoint < 8)
    {
        target = new Vector3(waypoints[currentWaypoint].position.x + 40, hoverHeight, waypoints[currentWaypoint].position.z + 60);
        
        //zooom += zoomSpeed * Time.deltaTime;
    }
    else if (currentWaypoint < 10)
    {
        target = new Vector3(waypoints[currentWaypoint].position.x + 30, hoverHeight, waypoints[currentWaypoint].position.z + 50);
        //zooom += zoomSpeed * Time.deltaTime;
    }
    else
    {
        target = new Vector3(waypoints[currentWaypoint].position.x + 10, hoverHeight, waypoints[currentWaypoint].position.z + 30);
    }
   

    // Bewege das Objekt nur, wenn es noch nicht nahe genug am Ziel ist
    if (Vector3.Distance(target, transform.position) > waypointRange)
    {
        CarController carController = new CarController();
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * 15 * Time.deltaTime;
         Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
    }
    else
    {
        // Erhöhe den aktuellen Waypoint, wenn das Ziel erreicht ist
        currentWaypoint += 1;
    }






       /* if (currentWaypoint == 0)
        {

            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 60, hoverHeight, waypoints[currentWaypoint].transform.position.z + 60);

            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 2)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 60, hoverHeight, waypoints[currentWaypoint].transform.position.z + 60);

            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

  
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 4)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 50, hoverHeight, waypoints[currentWaypoint].transform.position.z + 60);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 6)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 50, hoverHeight, waypoints[currentWaypoint].transform.position.z + 60);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 8)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 50, hoverHeight, waypoints[currentWaypoint].transform.position.z + 50);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 10)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 30, hoverHeight, waypoints[currentWaypoint].transform.position.z + 30);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                //cameraTransform.rotation= new Quaternion (0, 0, 0,0);
                currentWaypoint += 2;
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 12)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 30, hoverHeight, waypoints[currentWaypoint].transform.position.z + 30);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                currentWaypoint += 2;
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }

        else if (currentWaypoint == 14)
        {
            Vector3 target = new Vector3(waypoints[currentWaypoint].transform.position.x + 30, hoverHeight, waypoints[currentWaypoint].transform.position.z + 30);


            if (Vector3.Distance(target, transform.position) < waypointRange)

            {
                
            }

            Vector3 direction2 = (target - transform.position).normalized;
            transform.position += direction2 * 15 * Time.deltaTime;

        }*/



        //Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.yellow);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            followCar = true; // Aktiviere das Folgen des Autos
            carTarget = other.transform; // Setze das Auto als Zielobjekt
        }
    }
}
