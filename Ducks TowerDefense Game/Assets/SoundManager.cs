    using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour{
    public AudioMixer audioMixer;
    private bool isMuted = false;

// This method causes the sound manager to be a singleton, ensuring only one instance exists.
    public void ToggleMute(){
        isMuted = !isMuted;
        if (isMuted) audioMixer.SetFloat("Volume", -80f); // basically silent
        else audioMixer.SetFloat("Volume", 0f); // normal volume
    }
}//End of SoundManager.cs