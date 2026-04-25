using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    [Header("Assign your Pause UI Panel here")]
    public GameObject pausePanel;

    bool isPaused = false;

    void Update()
    {
        // Old Input System (caused your error):
        // if (Input.GetKeyDown(KeyCode.Escape))

        // New Input System (matches the rest of your project):
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Wire this to your "Main Menu" button's OnClick in the Inspector
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Always reset before loading a scene
        SceneManager.LoadScene("Main");
    }
}