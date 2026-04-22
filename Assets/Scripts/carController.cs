using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [Header ("Car settings")]
    public float AccelerationFactor = 15.0f;
    public float turnFactor = 3.75f;
    public float driftFactor  = 0.99f;
    public float maxSpeed = 20;


    float accelerationInput = 0;
    float steeringInput = 0;
    float rotationAngle = 0;
    float velocityVsUp = 0;

    Rigidbody2D carRigidbody2D;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
        void FixedUpdate()
    {
        ApplyEngineForce();

        KillorthogonalVelocity();

        ApplySeering();
    }

    void ApplyEngineForce()
    {

        velocityVsUp = Vector2.Dot(transform.up, carRigidbody2D.linearVelocity);
        
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }

        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
        {
            return;
        }

        if (carRigidbody2D.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }

        Vector2 engineForceVector = transform.up * accelerationInput * AccelerationFactor;

        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySeering()
    {
        float minSpeedBeforeAllowTurningFactor = (carRigidbody2D.linearVelocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);
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

        if(Mathf.Abs(GetLateralVelocity()) > 4.0f)
        {
            return true;                                                   
        }

        return false;
    }

    
}
