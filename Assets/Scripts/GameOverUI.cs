using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        AudioManager.Instance.StopDrift();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Wire to Restart Campaign button
    public void RestartCampaign()
    {
        Time.timeScale = 1f;
        levelManager.campaignMode = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Wire to Main Menu button
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}