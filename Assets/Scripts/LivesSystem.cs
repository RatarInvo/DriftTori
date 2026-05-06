//using UnityEngine;

//public class LivesSystem : MonoBehaviour
//{
//    public static LivesSystem Instance;

//    [Header("Lives")]
//    public int maxLives = 3;
//    int currentLives;

//    [Header("Heart Sprites above car - assign 3 heart GameObjects")]
//    public GameObject[] heartObjects;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        currentLives = maxLives;
//        UpdateHearts();
//    }

//    public void LoseLife()
//    {
//        if (currentLives <= 0) return;

//        currentLives--;
//        UpdateHearts();

//        if (currentLives <= 0)
//            OnGameOver();
//    }

//    public void ResetLives()
//    {
//        currentLives = maxLives;
//        UpdateHearts();
//    }

//    void UpdateHearts()
//    {
//        for (int i = 0; i < heartObjects.Length; i++)
//            heartObjects[i].SetActive(i < currentLives);
//    }

//    void OnGameOver()
//    {
//        ResetLives();

//        if (LevelManager.Instance != null)
//            LevelManager.Instance.RestartFromBeginning();
//        else
//            Debug.LogWarning("LevelManager instance not found on game over.");
//    }
//}