using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // Findet die Hauptkamera, die als Kind der Drohne angehängt ist
        cameraTransform = Camera.main.transform;
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
            FollowCar();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            followCar = true; // Aktiviere das Folgen des Autos
            carTarget = other.transform; // Setze das Auto als Zielobjekt
        }
    }
}
