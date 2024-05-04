using UnityEngine;

public class Car : MonoBehaviour
{

    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    [SerializeField] private float acceleration = 500f;
    [SerializeField] private float breakingForce = 300f;
    [SerializeField] private float maxTurnAngle = 15f;

    [SerializeField] private Transform[] waypoints;
    private int currentWaypoint = 0;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    private void FixedUpdate()
    {
        currentAcceleration = -acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
            currentBreakingForce = breakingForce;
        else
            currentBreakingForce = 0f;


        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontRight, frontRightTransform, true);
        UpdateWheel(frontLeft, frontLeftTransform, false);
        UpdateWheel(backRight, backRightTransform, true);
        UpdateWheel(backLeft, backLeftTransform, false);
    }

    private void UpdateWheel(WheelCollider col, Transform trans, bool isRightWheel)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;

        if (isRightWheel)
        {
            rotation *= Quaternion.Euler(0, 180, -90);
        }
        else
        {
            rotation *= Quaternion.Euler(0, 0, -90);
        }

        trans.rotation = rotation;
    }
}
