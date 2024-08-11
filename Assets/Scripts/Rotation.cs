using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private GameObject referenceObject;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private float initialRotX;
    private float initialRotZ;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float yRotation = referenceObject.transform.eulerAngles.y;

        transform.eulerAngles = new Vector3(90, yRotation, 0);

        transform.position = new Vector3(referenceObject.transform.position.x, initialPosition.y, referenceObject.transform.position.z);
    }
}
