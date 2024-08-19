using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NoRotation : MonoBehaviour
{
    [SerializeField] private GameObject referenceObject;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float initialY;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialY = initialRotation.y;
        //Debug.Log(initialRotation.eulerAngles.x + "," + initialRotation.eulerAngles.y + "," + initialRotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LateUpdate()
    {
       
        transform.position = new Vector3(referenceObject.transform.position.x, initialPosition.y, referenceObject.transform.position.z);

        Vector3 currentRotation = initialRotation.eulerAngles;
        float referenceRotationY = referenceObject.transform.rotation.eulerAngles.y;

        Quaternion newRotation = Quaternion.Euler(initialRotation.eulerAngles.x, initialRotation.y, initialRotation.eulerAngles.z);
        //transform.rotation = newRotation;

        //transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y + 219, currentRotation.z+81);
    }
}
