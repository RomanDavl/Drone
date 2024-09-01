using UnityEngine;
using UnityEngine.InputSystem.XR;

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
    private Rigidbody carRB;
    //private float currentTurnAngle = 0f;
    [SerializeField] private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    [SerializeField] private float maxSpeed = 80f;
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    void Start()
    {
        carRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        speed = Mathf.Min(Mathf.Abs(backRight.rpm * backRight.radius * 2f * Mathf.PI / 10f), maxSpeed);
        ApplyAcceleration();
        ApplyBraking();

        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(backRight, backRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
    }

    public void SetInput(float acceleration, float steeringIn)
    {
        currentAcceleration = acceleration;

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }      

        ApplySteering(steeringIn);

        slipAngle = Vector3.Angle(transform.forward, carRB.velocity - transform.forward);

        float movingDirection = Vector3.Dot(transform.forward, carRB.velocity);

        if (movingDirection < -0.5f && currentAcceleration > 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else if (movingDirection > 0.5f && currentAcceleration < 0)
        {
            currentBrakingForce = Mathf.Abs(currentAcceleration);
        }
        else
        {
            currentBrakingForce = 0;
        }

        ApplyAcceleration();
        ApplyBraking();
    }



    private void ApplyAcceleration()
    {
        if (speed < maxSpeed)
    {
            backRight.motorTorque = currentAcceleration * horsePower;
            backLeft.motorTorque = currentAcceleration * horsePower;
        }
    else
        {
            backRight.motorTorque = 0;
            backLeft.motorTorque = 0;
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime);
        }
    }

    private void ApplySteering(float steeringAngle)
    {
        //float steeringAngle = currentTurnAngle * steeringCurve.Evaluate(speed);

        if (Mathf.Abs(slipAngle) < 120f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, carRB.velocity + transform.forward, Vector3.up);
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

    private void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        //trans.position = position;

        trans.rotation = rotation;
    }
}
