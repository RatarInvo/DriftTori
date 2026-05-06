using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource musicSource;
    AudioSource sfxSource;
    AudioSource driftSource;

    float musicVolume = 0.5f;
    float sfxVolume = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;

        driftSource = gameObject.AddComponent<AudioSource>();
        driftSource.loop = true;
        driftSource.volume = sfxVolume;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // Don't restart same track
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayDrift(AudioClip clip)
    {
        if (driftSource.isPlaying || clip == null) return;
        driftSource.clip = clip;
        driftSource.Play();
    }

    public void StopDrift()
    {
        if (driftSource.isPlaying) driftSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
        driftSource.volume = volume;
    }
}