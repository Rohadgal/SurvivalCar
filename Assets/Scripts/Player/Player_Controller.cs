using UnityEngine;

/// <summary>
/// Controls the player's car, handling steering, acceleration, braking, and visual updates of the wheels.
/// </summary>
public class Player_Controller : MonoBehaviour {
    [SerializeField] private AudioClip startCar, carMove, carDrift;
    [SerializeField] private GameObject wheelEffectObj, wheelEffectObj1;
    
    // Adjustable parameters exposed in the Inspector
    [Header("Car Feel Variables")]
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private float steeringSensitivity = 0.8f;
    [SerializeField] private float adjustedBrakeForce = 1500f;
    [SerializeField] private float driftThreshold = 200f;

    [Header("Wheel Variables")]
    // References to wheel colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    // References to wheel transforms for visual representation
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;
    
    // Input axis names
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    // Input variables
    private float m_horizontalInput;
    private float m_verticalInput;

    // Steering and braking variables
    private float m_currentSteerAngle;
    private float m_currentBreakForce;
    private bool m_isBreaking;
    
    private void Start() {
        setWheelFriction(frontLeftWheelCollider);
        setWheelFriction(frontRightWheelCollider);
        setWheelFriction(backLeftWheelCollider);
        setWheelFriction(backRightWheelCollider);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        }
    }
    
    private void Update() {
        getInput();
    }
    
    private void FixedUpdate() {
        handleMotor();
        handleSteering();
        updateWheels();
        checkDrifting();
    }

    /// <summary>
    /// Sets the friction parameters for the given wheel collider to increase stability and reduce slipperiness.
    /// </summary>
    /// <param name="wheel">The wheel collider whose friction is being set.</param>
    private void setWheelFriction(WheelCollider wheel) {
        WheelFrictionCurve forwardFriction = wheel.forwardFriction;
        WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

        // Increase stiffness to reduce slipperiness
        forwardFriction.stiffness = 2.0f;
        sidewaysFriction.stiffness = 2.0f;

        wheel.forwardFriction = forwardFriction;
        wheel.sidewaysFriction = sidewaysFriction;
    }
    
    /// <summary>
    /// Increases the motor force by a specified amount.
    /// </summary>
    /// <param name="t_amountToIncrease">The amount by which to increase the motor force.</param>
    public void increaseMotorForce(float t_amountToIncrease) {
        motorForce += t_amountToIncrease;
    }

    /// <summary>
    /// Decreases the motor force by a specified amount.
    /// </summary>
    /// <param name="t_amountToDecrease">The amount by which to decrease the motor force.</param>
    public void decreaseMotorForce(float t_amountToDecrease) {
        motorForce -= t_amountToDecrease;
    }

    /// <summary>
    /// Retrieves the player's input for horizontal and vertical axes.
    /// </summary>
    private void getInput() {
        m_horizontalInput = Input.GetAxis(HORIZONTAL);
        m_verticalInput = Input.GetAxis(VERTICAL);
    }

    /// <summary>
    /// Applies motor torque to the front wheels to move the car.
    /// </summary>
    private void handleMotor() {
        float adjustedMotorForce = m_horizontalInput != 0 ? motorForce * 0.7f : motorForce;
        frontLeftWheelCollider.motorTorque = m_verticalInput * adjustedMotorForce;
        frontRightWheelCollider.motorTorque = m_verticalInput * adjustedMotorForce;

        m_currentBreakForce = m_isBreaking ? adjustedBrakeForce : 0f;
        if (m_isBreaking) {
            applyBraking();
        }
    }

    /// <summary>
    /// Applies the braking force to all wheel colliders.
    /// </summary>
    private void applyBraking() {
        frontLeftWheelCollider.brakeTorque = m_currentBreakForce;
        frontRightWheelCollider.brakeTorque = m_currentBreakForce;
        backLeftWheelCollider.brakeTorque = m_currentBreakForce;
        backRightWheelCollider.brakeTorque = m_currentBreakForce;
    }

    /// <summary>
    /// Steers the car based on the player's horizontal input.
    /// </summary>
    private void handleSteering() {
        m_currentSteerAngle = maxSteerAngle * m_horizontalInput * steeringSensitivity;
        frontLeftWheelCollider.steerAngle = m_currentSteerAngle;
        frontRightWheelCollider.steerAngle = m_currentSteerAngle;
    }

    /// <summary>
    /// Updates the visual representation of the wheels based on the wheel colliders.
    /// </summary>
    private void updateWheels() {
        updateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        updateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        updateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        updateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    /// <summary>
    /// Updates the visual position and rotation of a single wheel based on its wheel collider.
    /// </summary>
    /// <param name="t_wheelCollider">The wheel collider to update.</param>
    /// <param name="t_wheelTransform">The transform of the wheel for visual representation.</param>
    private void updateSingleWheel(WheelCollider t_wheelCollider, Transform t_wheelTransform) {
        Vector3 pos;
        Quaternion rot;
        t_wheelCollider.GetWorldPose(out pos, out rot);
        t_wheelTransform.rotation = rot;
        t_wheelTransform.position = pos;
    }

    /// <summary>
    /// Checks if the car is drifting based on the angular velocity difference between the front and rear wheels, 
    /// and activates or deactivates the drift effect accordingly.
    /// </summary>
    private void checkDrifting() {
        float angularVelocityFront = (frontLeftWheelCollider.rpm + frontRightWheelCollider.rpm) / 2f;
        float angularVelocityRear = (backLeftWheelCollider.rpm + backRightWheelCollider.rpm) / 2f;
        float angularVelocityDifference = angularVelocityFront - angularVelocityRear;
        bool isNotAcceleratingOrBraking = Mathf.Approximately(m_verticalInput, 0f);
        bool isNotTurning = Mathf.Approximately(m_horizontalInput, 0f);
        if (Mathf.Abs(angularVelocityDifference) > driftThreshold && isNotTurning && isNotAcceleratingOrBraking) {
            if (wheelEffectObj != null) {
                wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheelEffectObj1.GetComponentInChildren<TrailRenderer>().emitting = true;
            }
        } else {
            if (wheelEffectObj != null) {
                wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                wheelEffectObj1.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }
}
