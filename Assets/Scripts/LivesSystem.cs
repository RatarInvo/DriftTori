using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivesSystem : MonoBehaviour
{
    public static LivesSystem Instance;

    [Header("Lives")]
    public int maxLives = 3;
    int currentLives;
    public bool isGameOver = false;

    [Header("Hearts UI")]
    public GameObject heartsContainer;
    public GameObject[] heartObjects;

    [Header("Blink Animation")]
    public int blinkCount = 3;
    public float blinkInterval = 0.12f;
    public float showAfterBlinkDuration = 0.4f;

    Coroutine loseLifeCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLives = maxLives;
        heartsContainer.SetActive(false);    
        UpdateHearts();
    }

    public void LoseLife()
    {
        if (currentLives <= 0) return;

        if (loseLifeCoroutine != null)
            StopCoroutine(loseLifeCoroutine);

        loseLifeCoroutine = StartCoroutine(LoseLifeSequence());
    }

    IEnumerator LoseLifeSequence()
    {
        heartsContainer.SetActive(true);    

        int losingIndex = currentLives - 1;
        currentLives--;

        if (currentLives <= 0)
            isGameOver = true;

        // Update heart visuals
        for (int i = 0; i < heartObjects.Length; i++)
            heartObjects[i].SetActive(i < currentLives);

        // Blink the losing heart
        for (int i = 0; i < blinkCount; i++)
        {
            heartObjects[losingIndex].SetActive(false);
            yield return new WaitForSeconds(blinkInterval);
            heartObjects[losingIndex].SetActive(true);
            yield return new WaitForSeconds(blinkInterval);
        }

        heartObjects[losingIndex].SetActive(false);

        yield return new WaitForSeconds(showAfterBlinkDuration);

        heartsContainer.SetActive(false);

        if (currentLives <= 0)
            OnGameOver();
    }

    public void ResetLives()
    {
        if (loseLifeCoroutine != null)
        {
            StopCoroutine(loseLifeCoroutine);
            loseLifeCoroutine = null;
        }

        currentLives = maxLives;
        isGameOver = false;
        heartsContainer.SetActive(false);     
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartObjects.Length; i++)
            heartObjects[i].SetActive(i < currentLives);
    }

    void OnGameOver()
    {
        ResetLives();

        if (levelManager.campaignMode)
        {
            // Show game over screen instead of instant reset
            if (GameOverUI.Instance != null)
                GameOverUI.Instance.ShowGameOver();
        }
        else
        {
            // Normal mode keeps existing behaviour
            CarController car = FindAnyObjectByType<CarController>();
            if (car != null) car.ResetToSpawn();
        }
    }
}