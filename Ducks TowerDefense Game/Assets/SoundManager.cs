    using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            audioMixer.SetFloat("Volume", -80f); // basically silent
        }
        else
        {
            audioMixer.SetFloat("Volume", 0f); // normal volume
        }
    }
}