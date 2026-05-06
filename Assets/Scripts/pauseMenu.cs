using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    [Header("Assign your Pause UI Panel here")]
    public GameObject pausePanel;

    [Header("Volume Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    bool isPaused = false;

    void Start()
    {
        // Set sliders to current AudioManager volumes
        if (musicSlider != null)
        {
            musicSlider.value = 1f;
            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = 1f;
            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
        }
    }

    void Update()
    {
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

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}