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
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            AudioManager.Instance.SetMusicVolume(musicSlider.value);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            AudioManager.Instance.SetSFXVolume(sfxSlider.value);
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }
    }

    void OnMusicChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (GameOverUI.Instance != null && GameOverUI.Instance.gameOverPanel.activeSelf) return;
        if (UpgradeUI.Instance != null && UpgradeUI.Instance.upgradePanel.activeSelf) return;
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
        AudioManager.Instance.StopDrift();
        HUDManager.Instance.PauseTimer();
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Instance.UnPauseDrift();
        HUDManager.Instance.ResumeTimer();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }
}