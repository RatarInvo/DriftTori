using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AudioManager");
                _instance = go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    static AudioManager _instance;

    [Header("Scene Music - set these in the Inspector")]
    public AudioClip mainMenuMusic;
    public AudioClip gameMusic;

    AudioSource musicSource;
    AudioSource sfxSource;
    AudioSource driftSource;

    float musicVolume = 0.5f;
    float sfxVolume = 0.5f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
            PlayMusic(mainMenuMusic);
        else
            PlayMusic(gameMusic);
    }

    void SetupAudioSources()
    {
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
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, 1f);
    }

    public void InitDrift(AudioClip clip)
    {
        if (clip == null) return;
        driftSource.clip = clip;
        driftSource.volume = 0f;
        driftSource.loop = true;
        driftSource.Play();
    }

    public void SetDriftVolume(float targetVolume)
    {
        driftSource.volume = Mathf.Lerp(driftSource.volume, targetVolume * sfxVolume, Time.deltaTime * 8f);
    }

    public void StopDrift()
    {
        driftSource.volume = 0f;
        driftSource.Pause();
    }

    public void UnPauseDrift()
    {
        if (driftSource.clip != null)
            driftSource.UnPause();
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