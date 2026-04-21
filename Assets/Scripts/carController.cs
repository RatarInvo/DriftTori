using UnityEngine;

public class CarController : MonoBehaviour
{

    [Header ("Car settings")]
    public float AccelerationFactor = 30.0f;
    public float turnFactor = 3.5f;

    float steeringInput = 0;
    float rotationAngle = 0;

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

        ApplySeering();
    }

    void ApplyEngineForce()
    {
        Vector2 engineForceVector = transform.up * AccelerationFactor;

        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void ApplySeering()
    {
        rotationAngle -= steeringInput * turnFactor;

        carRigidbody2D.MoveRotation(rotationAngle);
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        
    }
}
