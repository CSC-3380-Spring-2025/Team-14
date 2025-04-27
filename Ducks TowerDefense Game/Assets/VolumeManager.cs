using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;

    void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        SetVolume(volumeSlider.value); // Set starting volume
    }

    void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}
