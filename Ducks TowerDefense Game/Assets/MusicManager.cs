    using UnityEngine;

// This script manages the music settings in the game.
public class MusicManager : MonoBehaviour{
    public static MusicManager Instance;
    private AudioSource audioSource;

// This method is called when the script instance is being loaded.
    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else Destroy(gameObject);
    }

// This method sets the audio volume.
    public void SetVolume(float volume) => audioSource.volume = volume;
    
// This method mutes or unmutes the audio.
    public void SetMute(bool isMuted) => audioSource.mute = isMuted;
    
// This method checks if the audio is muted.
    public bool IsMuted() => audioSource.mute;
}//End of MusicManager.cs