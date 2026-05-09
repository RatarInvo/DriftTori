using UnityEngine;

public class wheelTrail : MonoBehaviour
{
    CarController carController;
    TrailRenderer trailRenderer;

    [Header("Drift Sound")]
    public AudioClip driftClip;

    bool initialized = false;

    void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (driftClip != null)
        {
            AudioManager.Instance.InitDrift(driftClip);
            initialized = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;

        if (carController.isTireScreeching(out float lateralVelocity))
        {
            trailRenderer.emitting = true;

            float driftIntensity = Mathf.Clamp01(Mathf.Abs(lateralVelocity) / 10f);
            AudioManager.Instance.SetDriftVolume(driftIntensity);
        }
        else
        {
            trailRenderer.emitting = false;
            AudioManager.Instance.SetDriftVolume(0f);
        }
    }
}