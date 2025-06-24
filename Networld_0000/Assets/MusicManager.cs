using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelMusicMapping
{
    public string levelName;
    public AudioClip musicClip;
}

[System.Serializable]
public class AudioSettings
{
    public float musicVolume;
    public float sfxVolume;
    public string lastLevelMusic; // Name of the last played music/level
}

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    [Header("Music")]
    public AudioClip backgroundMusic;
    [SerializeField] private List<LevelMusicMapping> levelMusicMappings;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Slider musicSlider;
    [SerializeField] private UnityEngine.UI.Slider sfxSlider;

    private const string SettingsFile = "audio_settings.json";
    private AudioSettings currentSettings = new AudioSettings();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackGroundMusic(false, backgroundMusic);
        }

        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });

        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(sfxSlider.value); });

        // Set sliders to saved values
        if (musicSlider != null)
            musicSlider.value = currentSettings.musicVolume;
        if (sfxSlider != null)
            sfxSlider.value = currentSettings.sfxVolume;
    }

    public static void SetVolume(float volume)
    {
        if (instance == null) return;
        instance.audioSource.volume = volume;
        instance.currentSettings.musicVolume = volume;
        instance.SaveSettings();
    }

    public void SetSliders(UnityEngine.UI.Slider music, UnityEngine.UI.Slider sfx)
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveAllListeners();
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveAllListeners();

        musicSlider = music;
        sfxSlider = sfx;

        if (musicSlider != null)
        {
            musicSlider.value = currentSettings.musicVolume;
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
            // Ensure the AudioSource volume is set immediately
            SetVolume(musicSlider.value);
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = currentSettings.sfxVolume;
            sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(sfxSlider.value); });
        }
    }

    public void SetSFXVolume(float volume)
    {
        // Implement SFX volume logic in your SFX manager as needed
        currentSettings.sfxVolume = volume;
        SaveSettings();
    }

    public void PlayMusicForLevel(string levelName)
    {
        var mapping = levelMusicMappings.Find(m => m.levelName == levelName);
        if (mapping != null && mapping.musicClip != null)
        {
            PlayBackGroundMusic(true, mapping.musicClip);
            currentSettings.lastLevelMusic = levelName;
            SaveSettings();
        }
    }

    public void PlayBackGroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioSource == null)
        {
            Debug.LogError("MusicManager: AudioSource is not assigned!");
            return;
        }

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if (audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }

    public void PauseBackGroundMusic()
    {
        audioSource.Pause();
    }

    private void SaveSettings()
    {
        string json = JsonUtility.ToJson(currentSettings);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, SettingsFile), json);
    }

    private void LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath, SettingsFile);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            currentSettings = JsonUtility.FromJson<AudioSettings>(json);
        }
        else
        {
            currentSettings = new AudioSettings { musicVolume = 1f, sfxVolume = 1f, lastLevelMusic = "" };
        }
    }
}