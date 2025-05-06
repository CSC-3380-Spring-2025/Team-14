using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
// Loads the saved volume preference and applies it--------------------------------------------------------------------
    void Start()
    {
        // Optional: Load saved volume
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            volumeSlider.value = savedVolume;
            SetVolume(savedVolume);
        }
    }
// Sets the volume and saves the preference--------------------------------------------------------------------
    public void SetVolume(float volume)
    {
        // Convert to decibels
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("MasterVolume", dB);

        // Save volume
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
//--------------------------------------------------------------------
}