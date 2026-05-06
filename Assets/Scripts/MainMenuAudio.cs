using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public AudioClip menuMusic;

    void Start()
    {
        if (menuMusic != null)
            AudioManager.Instance.PlayMusic(menuMusic);
    }
}