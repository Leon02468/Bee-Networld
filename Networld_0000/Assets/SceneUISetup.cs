using UnityEngine;
using UnityEngine.UI;

public class SceneUISetup : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        var musicManager = FindFirstObjectByType<MusicManager>();
        if (musicManager != null)
        {
            musicManager.SetSliders(musicSlider, sfxSlider);
        }

        var sfxManager = FindFirstObjectByType<SoundEffectManager>();
        if (sfxManager != null && sfxSlider != null)
        {
            sfxManager.SetSFXSlider(sfxSlider);
        }
    }
}