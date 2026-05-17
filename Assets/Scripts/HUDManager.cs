using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("HUD Text Elements")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI speedText;

    [Header("References")]
    public Rigidbody2D carRigidbody;

    float elapsedTime = 0f;
    bool timerRunning = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeText();
        }

        UpdateSpeedText();
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 100f) % 100f);
        timeText.text = $"Time: {minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    void UpdateSpeedText()
    {
        if (carRigidbody == null) return;

        float kmh = carRigidbody.linearVelocity.magnitude * 3.6f;
        speedText.text = $"Speed: {Mathf.RoundToInt(kmh)} km/h";
    }

    public void StartTimer()
    {
        timerRunning = true;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = false;
        UpdateTimeText();
    }

    public void PauseTimer() => timerRunning = false;
    public void ResumeTimer() => timerRunning = true;
}