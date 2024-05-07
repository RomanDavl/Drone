using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    [SerializeField] private float horsePower;
    [SerializeField] private float brakingForce;
    [SerializeField] private AnimationCurve steeringCurve;
    [SerializeField] private float slipAngle;
    [SerializeField] private float maxSteeringAngle;

    private float currentAcceleration = 0f;
    private float currentBrakingForce = 0f;
    private Rigidbody playerRB;
    //private float currentTurnAngle = 0f;
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }

    IEnumerator Start()
    {
        playerRB = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(UpdateWheels());
    }

    private void Update()
    {
        //CheckInput();
        speed = Mathf.Abs(frontRight.rpm * frontRight.radius * 2f * Mathf.PI / 10f);
        ApplyAcceleration();
        ApplyBraking();
        
        /*UpdateWheel(frontRight, frontRightTransform, true);
        UpdateWheel(frontLeft, frontLeftTransform, false);
        UpdateWheel(backRight, backRightTransform, true);
        UpdateWheel(backLeft, backLeftTransform, false);*/
    }

    public void SetInput(float acceleration, float steeringIn)
    {
        currentAcceleration = acceleration;

        ApplySteering(steeringIn);

        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);

        if (movingDirection < -0.5f && currentAcceleration < 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else if (movingDirection > 0.5f && currentAcceleration > 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else
        {
            currentBrakingForce = 0;
        }
    }



    private void ApplyAcceleration()
    {
        backRight.motorTorque = -currentAcceleration * horsePower;
        backLeft.motorTorque = -currentAcceleration * horsePower;
    }

    private void ApplySteering(float steeringAngle)
    {
        //float steeringAngle = currentTurnAngle * steeringCurve.Evaluate(speed);

        if (slipAngle < 120f)
        {
            steeringAngle += Vector3.SignedAngle(-transform.forward, playerRB.velocity + -transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -maxSteeringAngle, maxSteeringAngle);
        frontLeft.steerAngle = steeringAngle;
        frontRight.steerAngle = steeringAngle;

    }

    private void ApplyBraking()
    {
        frontRight.brakeTorque = currentBrakingForce * brakingForce * 0.7f;
        frontLeft.brakeTorque = currentBrakingForce * brakingForce * 0.7f;
        backRight.brakeTorque = currentBrakingForce * brakingForce * 0.3f;
        backLeft.brakeTorque = currentBrakingForce * brakingForce * 0.3f;
    }

    IEnumerator UpdateWheels()
    {
        while (true)
        {
            ApplyAcceleration();
            ApplyBraking();

            UpdateWheel(frontRight, frontRightTransform, true);
            UpdateWheel(frontLeft, frontLeftTransform, false);
            UpdateWheel(backRight, backRightTransform, true);
            UpdateWheel(backLeft, backLeftTransform, false);
            yield return null;
        }
    }

    private void UpdateWheel(WheelCollider col, Transform trans, bool isRightWheel)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;

        if (isRightWheel)
        {
            rotation *= Quaternion.Euler(0, 0, 90);
        }
        else
        {
            rotation *= Quaternion.Euler(90, 90, 0);
        }

        trans.rotation = rotation;
    }

    /*private void CheckInput()
    {
        currentAcceleration = Input.GetAxis("Vertical");
        currentTurnAngle = Input.GetAxis("Horizontal");

        slipAngle = Vector3.Angle(transform.forward, playerRB.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, playerRB.velocity);
        if (movingDirection < -0.5f && currentAcceleration < 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else if (movingDirection > 0.5f && currentAcceleration > 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else
        {
            currentBrakingForce = 0;
        }
    }*/
}
