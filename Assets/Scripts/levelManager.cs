using UnityEngine;
using UnityEngine.SceneManagement;

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

        if (levelSpawnPoints.Length > 0)
        {
            car.TeleportTo(levelSpawnPoints[0].position, levelSpawnPoints[0].eulerAngles.z);
            LevelTitleUI.Instance.ShowTitle(0); // Show Level1 title on game start
        }
    }

    public void AdvanceToNextLevel()
    {
        currentLevel++;

        if (currentLevel >= levelSpawnPoints.Length)
        {
            car.carStarted = false;
            car.engineMultiplier = 0f;
            car.isFinishing = false;
            car.ReleaseBrake();
            SceneManager.LoadScene("Main");
            Debug.Log("All levels complete!");
            return;
        }

        Transform spawn = levelSpawnPoints[currentLevel];
        car.TeleportTo(spawn.position, spawn.eulerAngles.z);
        car.ReleaseBrake();
        car.isFinishing = false;
        car.carStarted = false;
        car.engineMultiplier = 0f;

        LevelTitleUI.Instance.ShowTitle(currentLevel);
    }
}