using UnityEngine;

public class wheelTrail : MonoBehaviour
{

    CarController carController;
    TrailRenderer trailRenderer;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (carController.isTireScreeching(out float lateralVelocity))
        {
            trailRenderer.emitting = true;
        } else { trailRenderer.emitting = false; }
    }
}
