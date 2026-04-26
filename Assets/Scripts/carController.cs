using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float AccelerationFactor = 15.0f;
    public float turnFactor = 3.75f;
    public float driftFactor = 0.99f;
    public float maxSpeed = 20;

    [Header("Engine Ramp Up")]
    [Tooltip("How many seconds to reach full acceleration from a standstill")]
    public float engineRampDuration = 2.5f;

    public bool carStarted = false;
    public float engineMultiplier = 0f;
    public bool isFinishing = false;

    bool isBraking = false;
    float brakeDeceleration = 15f;

    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    // Spawn state
    Vector2 spawnPosition;
    float spawnRotation;

    Rigidbody2D carRigidbody2D;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rotationAngle = transform.eulerAngles.z;

        spawnPosition = transform.position;
        spawnRotation = rotationAngle;
    }

    public void StartCar()
    {
        carStarted = true;
    }

    // Add this public method to trigger braking from FinishLine
    public void BrakeToStop()
    {
        isBraking = true;
    }

    // Add this public method so LevelManager can release the brake after teleport
    public void ReleaseBrake()
    {
        isBraking = false;
    }

    void FixedUpdate()
    {
        if (!carStarted)
            return;

        if (engineMultiplier < 1f)
        {
            engineMultiplier = Mathf.MoveTowards(
                engineMultiplier,
                1f,
                Time.fixedDeltaTime / engineRampDuration
            );
        }

        if (isBraking)
        {
            // Rapidly drain velocity to zero
            carRigidbody2D.linearVelocity = Vector2.MoveTowards(
                carRigidbody2D.linearVelocity,
                Vector2.zero,
                brakeDeceleration * Time.fixedDeltaTime
            );
            return; // Skip engine and steering while braking
        }

        ApplyEngineForce();
        KillorthogonalVelocity();
        ApplySeering();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (isFinishing) 
                return;

            ResetToSpawn();
        }
    }

    void ResetToSpawn()
    {
        TeleportTo(spawnPosition, spawnRotation);
        engineMultiplier = 0f;
        carStarted = false;
    }

    void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.linearVelocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0) return;
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) return;
        if (carRigidbody2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;

        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationFactor * engineMultiplier;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySeering()
    {
        float minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(carRigidbody2D.linearVelocity.magnitude / 8);
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void KillorthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);
        carRigidbody2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void TeleportTo(Vector2 position, float zRotation)
    {
        carRigidbody2D.linearVelocity = Vector2.zero;
        carRigidbody2D.angularVelocity = 0f;
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
        rotationAngle = zRotation;
        spawnPosition = position;
        spawnRotation = zRotation;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.linearVelocity);
    }

    public bool isTireScreeching(out float lateralVelocity)
    {
        lateralVelocity = GetLateralVelocity();
        return Mathf.Abs(lateralVelocity) > 4.0f;
    }
}