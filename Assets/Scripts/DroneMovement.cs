using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Transform carTarget; // Das Auto-Zielobjekt

    private Transform cameraTransform; // Die Kamera, die an der Drohne befestigt ist
    
    int i = 0;

    [SerializeField] private WaypointContainer waypointContainer;
    private List<Transform> waypoints;
    private int currentWaypoint;
    private float currentAngle;
    private float waypointRange;

    private float zooom = 1;
    public float zoomSpeed = 5f;

    [SerializeField] private CarController auto;
    private int currentShot = 0;

    float time;
    Boolean isTimeSet;
    Boolean atWaypoint = false;

    Boolean timerButton = true;
    Boolean fliegeZumAnderenOrt;
    Boolean angekommen;




    void Start()
    {
        // Findet die Hauptkamera, die als Kind der Drohne angehängt ist
        cameraTransform = Camera.main.transform;

        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0;
        waypointRange = 3f;
        isTimeSet = false;
        float total = waypointContainer.GetDistanceTotal();
        Debug.Log("Die Distanz der kompletten Strecke beträgt: " + total);

        float distanceTillMeetingpoint = waypointContainer.GetDistanceOfList(waypointContainer.GetAllWaypointsUpToWaypoint(targetWaypoint));
        Debug.Log("Die Distanz bis zum Treffpunkt beträgt: " + distanceTillMeetingpoint);

        float distanceOfDrohenshot = waypointContainer.GetDistanceOfList(waypointContainer.GetAllWaypointsStartingAtWaypoint(targetWaypoint));
        Debug.Log("Die Distanz des Drohnenshots beträgt: " + distanceOfDrohenshot);

        DrohnenshotTime();

        fliegeZumAnderenOrt = true;
        angekommen = false;


    }

    void FixedUpdate()
    {
        if (fliegeZumAnderenOrt == true)
        {

            if (angekommen == false)
            {

                Vector3 target = new Vector3(waypoints[currentWaypoint].position.x, hoverHeight, waypoints[currentWaypoint].position.z);

                if (Vector3.Distance(target, transform.position) > waypointRange)
                {
                    Vector3 direction = (target - transform.position).normalized;
                    transform.position += direction * 200 * Time.deltaTime;
                }

                else
                {
                    angekommen = true;
                    fliegeZumAnderenOrt = false;

                }

            }

        }
        else
        {

            if (timerButton)
            {

                if (!atWaypoint)
                {
                    FlyToWaypointWithTimer();

                }



                if (!isTimeSet)
                {
                    time = CarAtWaypointTime();
                    isTimeSet = true;
                    Debug.Log("Time is set: " + time);
                }

                if (isTimeSet)
                {
                    if (time > 0)
                    {
                        time -= Time.deltaTime;
                        Debug.Log("Time remaining: " + time + " seconds");

                    }
                    else
                    {

                        Debug.Log("Drone is moving");


                        switch (currentShot)
                        {
                            case 1:
                                Drohnenshot1();
                                break;
                            case 2:
                                Drohnenshot2();
                                break;
                            case 3:
                                Drohnenshot3();
                                break;
                            case 4:
                                FollowCarDrohnenshot();
                                break;
                            default:
                                FollowCar();
                                break;
                        }

                    }
                }
            }
            else
            {


                if (followCar)
                {
                    switch (currentShot)
                    {
                        case 1:
                            Drohnenshot1();
                            break;
                        case 2:
                            Drohnenshot2();
                            break;
                        case 3:
                            Drohnenshot3();
                            break;
                        case 4:
                            FollowCarDrohnenshot();
                            break;
                        default:
                            FollowCar();
                            break;
                    }
                }
                else
                {
                    FlyToWaypointWithoutTimer();
                }

            }

        }


    }
    public void SetShot(int shotNumber){
        currentShot = shotNumber;
    }
    public void SetTimerButton(Boolean timer){
        timerButton=timer;
    }

    // gibt die Zeit aus wie lange das Auto braucht, um am Waypoint zu sein, ab wann die Drohne das Auto verfolgt
    public float CarAtWaypointTime()
    {

        float time = 10; // time ist auf 10 Sekunden gestellt, da das Auto noch beschleunigen muss

        float distanceTillMeetingpoint = waypointContainer.GetDistanceOfList(waypointContainer.GetAllWaypointsUpToWaypoint(targetWaypoint));
        float speed = auto.MaxSpeed;

        if (distanceTillMeetingpoint ==0)
        {
            time += 0;
        }
        else
        {
            time += distanceTillMeetingpoint / (speed * 0.125f);
        }
        
        

        return time;

    }


    //gibt die Zeit aus wie lange der Drohnenshot dauert
    public float DrohnenshotTime()
    {
        float drohnenshottime;

        float drohenshotdistance = waypointContainer.GetDistanceOfList(waypointContainer.GetAllWaypointsStartingAtWaypoint(targetWaypoint));

        drohnenshottime = drohenshotdistance / 12;
        
        //Debug.Log("Die Zeit des Drohnenshots beträgt: " + drohnenshottime);

        return drohnenshottime;

    }


    void FlyToWaypointWithoutTimer()
    {
        if (targetWaypoint != null)
        {
            // Zielposition berechnen, die 10 Meter über dem Waypoint liegt
            Vector3 targetPosition = new Vector3(targetWaypoint.position.x, targetWaypoint.position.y + hoverHeight, targetWaypoint.position.z);
            // Richtung zur Zielposition berechnen
            Vector3 direction = (targetPosition - transform.position).normalized;
            //Drehung der drohne in richtung des zielpunkts
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

    void FlyToWaypointWithTimer()
    {
        if (targetWaypoint != null)
        {
            // Zielposition berechnen, die 10 Meter über dem Waypoint liegt
            Vector3 targetPosition = new Vector3(targetWaypoint.position.x, targetWaypoint.position.y + hoverHeight, targetWaypoint.position.z + 60);

            Vector3 direction = (targetPosition - transform.position).normalized;
            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed * Time.deltaTime, 0.0f);
            //transform.rotation = Quaternion.LookRotation(targetWaypoint.position);

            transform.position += direction * 50 * Time.deltaTime;

            // Optional: Wenn die Drohne die Zielposition erreicht, könnte sie stoppen
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                //Debug.Log("Drohne hat den Waypoint erreicht und schwebt darüber!");
                atWaypoint = true; ; // Stopp die Drohne 
                return;
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
            transform.position += direction * 11 * Time.deltaTime;

            // Kamera soll stets auf das Auto gerichtet sein
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
    }

    void FollowCarDrohnenshot()
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
            if (currentWaypoint < 2)
            {
                target = new Vector3(waypoints[currentWaypoint+1].position.x, hoverHeight, waypoints[currentWaypoint+1].position.z);
            }
            else
            {
                target = new Vector3(waypoints[currentWaypoint].position.x, hoverHeight, waypoints[currentWaypoint].position.z);
                //zooom += zoomSpeed * Time.deltaTime;
            }

        }
        else if (currentWaypoint < 8)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x , hoverHeight, waypoints[currentWaypoint].position.z );

            //zooom += zoomSpeed * Time.deltaTime;
        }
        else if (currentWaypoint < 12)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x , hoverHeight, waypoints[currentWaypoint].position.z);
            //zooom += zoomSpeed * Time.deltaTime;
        }
        else
        {
            target = new Vector3(waypoints[currentWaypoint].position.x , hoverHeight, waypoints[currentWaypoint].position.z);
        }


        // Bewege das Objekt nur, wenn es noch nicht nahe genug am Ziel ist
        if (Vector3.Distance(target, transform.position) > waypointRange)
        {

            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 12 * Time.deltaTime;
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
        else
        {
            // Erhöhe den aktuellen Waypoint, wenn das Ziel erreicht ist
            currentWaypoint += 1;
        }
    }



    void Drohnenshot1()
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
            target = new Vector3(waypoints[currentWaypoint].position.x + 50, hoverHeight, waypoints[currentWaypoint].position.z);
            //zooom += zoomSpeed * Time.deltaTime;

        }
        else if (currentWaypoint < 8)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 40, hoverHeight, waypoints[currentWaypoint].position.z + 60);

            //zooom += zoomSpeed * Time.deltaTime;
        }
        else if (currentWaypoint < 12)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 30, hoverHeight, waypoints[currentWaypoint].position.z + 50);
            //zooom += zoomSpeed * Time.deltaTime;
        }
        else
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 10, hoverHeight, waypoints[currentWaypoint].position.z);
        }


        // Bewege das Objekt nur, wenn es noch nicht nahe genug am Ziel ist
        if (Vector3.Distance(target, transform.position) > waypointRange)
        {
            
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 13 * Time.deltaTime;
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
        else
        {
            // Erhöhe den aktuellen Waypoint, wenn das Ziel erreicht ist
            currentWaypoint += 1;
        }

    }

    void Drohnenshot2 ()
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
            target = new Vector3(waypoints[currentWaypoint].position.x + 50, hoverHeight+50, waypoints[currentWaypoint].position.z - 80);
            //zooom += zoomSpeed * Time.deltaTime;

        }
        else if (currentWaypoint < 8)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 40, hoverHeight+40, waypoints[currentWaypoint].position.z - 70);

            //zooom += zoomSpeed * Time.deltaTime;
        }
        else if (currentWaypoint < 12)
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 30, hoverHeight, waypoints[currentWaypoint].position.z - 60);
            //zooom += zoomSpeed * Time.deltaTime;
        }
        else
        {
            target = new Vector3(waypoints[currentWaypoint].position.x + 10, hoverHeight-20, waypoints[currentWaypoint].position.z -20);
        }


        // Bewege das Objekt nur, wenn es noch nicht nahe genug am Ziel ist
        if (Vector3.Distance(target, transform.position) > waypointRange)
        {
            
            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 13 * Time.deltaTime;
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
        else
        {
            // Erhöhe den aktuellen Waypoint, wenn das Ziel erreicht ist
            currentWaypoint += 1;
        }
    }

    void Drohnenshot3()
    {
        if (currentWaypoint >= waypoints.Count)
        {
            // Wenn alle Waypoints erreicht sind, kann die Funktion beendet werden
            return;
        }

        // Berechne das Ziel basierend auf dem aktuellen Waypoint
        Vector3 target;

        if (currentWaypoint < 5)
        {
            target = new Vector3(waypoints[7].position.x + 200, hoverHeight + 100, waypoints[7].position.z - 60);
            //zooom += zoomSpeed * Time.deltaTime;

        }
        else if (currentWaypoint < 9)
        {
            target = new Vector3(waypoints[10].position.x, hoverHeight + 50, waypoints[10].position.z - 70);

            //zooom += zoomSpeed * Time.deltaTime;
        }
        
        else
        {
            target = new Vector3(waypoints[5].position.x -100, hoverHeight +200, waypoints[5].position.z - 10);
        }


        // Bewege das Objekt nur, wenn es noch nicht nahe genug am Ziel ist
        if (Vector3.Distance(target, transform.position) > waypointRange)
        {

            Vector3 direction = (target - transform.position).normalized;
            transform.position += direction * 18 * Time.deltaTime;
            Vector3 cameraDirection = (carTarget.position - cameraTransform.position).normalized;
            Quaternion cameraTargetRotation = Quaternion.LookRotation(cameraDirection);
            cameraTransform.rotation = Quaternion.RotateTowards(cameraTransform.rotation, cameraTargetRotation, rotationSpeed * Time.deltaTime * 100);
        }
        else
        {
            // Erhöhe den aktuellen Waypoint, wenn das Ziel erreicht ist
            currentWaypoint += 1;
        }
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
