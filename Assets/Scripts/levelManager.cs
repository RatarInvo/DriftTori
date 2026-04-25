using UnityEngine;

public class levelManager : MonoBehaviour
{
    public static levelManager Instance;

    [Header("Level Spawn Points (one per level, in order)")]
    public Transform[] levelSpawnPoints;

    int currentLevel = 0;
    CarController car;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        car = FindAnyObjectByType<CarController>();

        // Snap car to level 0 spawn on start
        if (levelSpawnPoints.Length > 0)
            car.TeleportTo(levelSpawnPoints[0].position, levelSpawnPoints[0].eulerAngles.z);
    }

    public void AdvanceToNextLevel()
    {
        currentLevel++;

        if (currentLevel >= levelSpawnPoints.Length)
        {
            Debug.Log("All levels complete!");
            // Hook in a win screen here later if needed
            return;
        }

        Transform spawn = levelSpawnPoints[currentLevel];
        car.TeleportTo(spawn.position, spawn.eulerAngles.z);
    }
}