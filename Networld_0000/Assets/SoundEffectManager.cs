using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager instance;
    private AudioSource audioSource;
    private SoundEffectLibrary soundEffectLibrary;

    [SerializeField] private Slider sfxSlider;

    // Listeners for SFX volume changes
    private List<Action<float>> sfxVolumeListeners = new List<Action<float>>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
            sfxSlider.value = audioSource != null ? audioSource.volume : 1f;
        }
    }

    public void SetSFXSlider(Slider slider)
    {
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveAllListeners();

        sfxSlider = slider;

        if (sfxSlider != null)
        {
            sfxSlider.value = audioSource != null ? audioSource.volume : 1f;
            sfxSlider.onValueChanged.AddListener(delegate { OnSliderValueChanged(); });
            SetVolume(sfxSlider.value);
        }
    }

    public void OnSliderValueChanged()
    {
        if (sfxSlider != null)
            SetVolume(sfxSlider.value);
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
            audioSource.volume = volume;

        // Notify all listeners
        foreach (var listener in sfxVolumeListeners)
            listener?.Invoke(volume);
    }

    public float GetCurrentVolume()
    {
        return audioSource != null ? audioSource.volume : 1f;
    }

    public void RegisterSfxVolumeListener(Action<float> listener)
    {
        if (!sfxVolumeListeners.Contains(listener))
            sfxVolumeListeners.Add(listener);
    }

    public void UnregisterSfxVolumeListener(Action<float> listener)
    {
        if (sfxVolumeListeners.Contains(listener))
            sfxVolumeListeners.Remove(listener);
    }

    public static void Play(string soundName)
    {
        if (instance == null || instance.soundEffectLibrary == null || instance.audioSource == null)
            return;

        AudioClip audioClip = instance.soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            instance.audioSource.PlayOneShot(audioClip);
        }
    }
}