using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class levelManager : MonoBehaviour
{
    public static levelManager Instance;

    [Header("Level Spawn Points")]
    public Transform[] levelSpawnPoints;

    CarController car;

    // NORMAL MODE
    int currentLevel = 0;

    // CAMPAIGN MODE
    int campaignLevel = 1;
    int currentMapIndex;
    List<int> playedMaps = new List<int>();

    // TRUE = random campaign mode
    public static bool campaignMode = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        car = FindAnyObjectByType<CarController>();

        if (campaignMode)
        {
            StartRandomLevel();
        }
        else
        {
            StartNormalLevel();
        }
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.qKey.wasPressedThisFrame &&
            (Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed) &&
            (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed))
        {
            AdvanceToNextLevel();
        }
    }


    //Normal mode
    void StartNormalLevel()
    {
        Transform spawn = levelSpawnPoints[currentLevel];
        car.TeleportTo(spawn.position, spawn.eulerAngles.z);
        car.ReleaseBrake();
        car.isFinishing = false;
        car.carStarted = false;
        car.engineMultiplier = 0f;
        LivesSystem.Instance.ResetLives();
        LevelTitleUI.Instance.ShowTitle(currentLevel);
        CoinSpawner.Instance.SpawnCoinsForLevel(currentLevel, spawn.position); // Add this
    }

    //camp mode
    void StartRandomLevel()
    {
        if (playedMaps.Count >= levelSpawnPoints.Length)
            playedMaps.Clear();

        List<int> availableMaps = new List<int>();
        for (int i = 0; i < levelSpawnPoints.Length; i++)
        {
            if (!playedMaps.Contains(i))
                availableMaps.Add(i);
        }

        currentMapIndex = availableMaps[Random.Range(0, availableMaps.Count)];
        playedMaps.Add(currentMapIndex);

        Transform spawn = levelSpawnPoints[currentMapIndex];
        car.TeleportTo(spawn.position, spawn.eulerAngles.z);
        car.ReleaseBrake();
        car.isFinishing = false;
        car.carStarted = false;
        car.engineMultiplier = 0f;
        LevelTitleUI.Instance.ShowTitle(campaignLevel - 1);
        CoinSpawner.Instance.SpawnCoinsForLevel(currentMapIndex, spawn.position);
    }

    public void AdvanceToNextLevel()
    {
        if (campaignMode)
        {
            campaignLevel++;
            StartRandomLevel();
        }
        else
        {
            currentLevel++;

            if (currentLevel >= levelSpawnPoints.Length)
            {
                car.carStarted = false;
                car.engineMultiplier = 0f;
                car.isFinishing = false;
                car.ReleaseBrake();

                SceneManager.LoadScene("Main");
                return;
            }

            StartNormalLevel();
        }
    }

    public void RestartFromBeginning()
    {
        currentLevel = 0;
        Transform spawn = levelSpawnPoints[0];
        car.TeleportTo(spawn.position, spawn.eulerAngles.z);
        car.ReleaseBrake();
        car.isFinishing = false;
        car.carStarted = false;
        car.engineMultiplier = 0f;
        LevelTitleUI.Instance.ShowTitle(0);
        CoinSpawner.Instance.SpawnCoinsForLevel(0, spawn.position);
    }
}