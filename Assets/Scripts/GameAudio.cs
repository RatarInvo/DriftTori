using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClip gameMusic;

    void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager not found - start from Main scene or add AudioManager to Game scene too.");
            return;
        }

        if (gameMusic != null)
            AudioManager.Instance.PlayMusic(gameMusic);
    }
}