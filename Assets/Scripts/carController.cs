using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float AccelerationFactor = 15.0f;
    public float turnFactor = 3.75f;
    public float driftFactor = 0.99f;
    public float maxSpeed = 20;

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
        // FIX 1: Sync rotationAngle to wherever the car actually starts in the scene
        // so MoveRotation() doesn't snap it on frame 1
        rotationAngle = transform.eulerAngles.z;

        // FIX 2: Record spawn state for wall-reset
        spawnPosition = transform.position;
        spawnRotation = rotationAngle;
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        KillorthogonalVelocity();
        ApplySeering();
    }

    // FIX 2: Called automatically by Unity when this collider hits another
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            ResetToSpawn();
        }
    }

    // Add this public method — called by LevelManager for both level transitions and wall resets
    public void TeleportTo(Vector2 position, float zRotation)
    {
        carRigidbody2D.linearVelocity = Vector2.zero;
        carRigidbody2D.angularVelocity = 0f;

        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);

        // Update internal angle and spawn state so steering and wall-reset both stay in sync
        rotationAngle = zRotation;
        spawnPosition = position;
        spawnRotation = zRotation;
    }

    void ResetToSpawn()
    {
        TeleportTo(spawnPosition, spawnRotation);

        // Stop all momentum
        carRigidbody2D.linearVelocity = Vector2.zero;
        carRigidbody2D.angularVelocity = 0f;

        // Teleport back
        transform.position = spawnPosition;
        transform.rotation = Quaternion.Euler(0, 0, spawnRotation);

        // Re-sync internal angle tracker so steering doesn't snap
        rotationAngle = spawnRotation;
    }

    void ApplyEngineForce()
    {
        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.linearVelocity);

        if (velocityVsUp > maxSpeed && accelerationInput > 0) return;
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0) return;
        if (carRigidbody2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0) return;

        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationFactor;
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