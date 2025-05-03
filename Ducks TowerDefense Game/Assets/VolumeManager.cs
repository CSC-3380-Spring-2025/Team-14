using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    public Slider volumeSlider;      // Drag your volume slider here
    public Toggle muteToggle;        // Drag your mute toggle here
    public AudioSource audioSource;  // Drag your AudioSource here (or on same GameObject)

    private float lastVolume = 1f;   // Stores last volume before muting

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it between scenes
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate music players
        }
    }

    void Start()
    {
        // Set starting volume to 50%
        volumeSlider.value = 0.5f;

        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(SetMute);

        SetVolume(volumeSlider.value);     // Apply initial volume
        muteToggle.isOn = false;           // Start unmuted
    }

    public void SetVolume(float value)
    {
        if (!muteToggle.isOn)
        {
            audioSource.volume = value;
            lastVolume = value; // Store last good volume
        }
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
        {
            lastVolume = volumeSlider.value;  // Save the slider's current value
            audioSource.volume = 0f;
        }
        else
        {
            audioSource.volume = lastVolume;
        }
    }
}