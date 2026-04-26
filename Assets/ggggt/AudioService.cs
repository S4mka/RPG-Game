using UnityEngine;

public class AudioService : MonoBehaviour , IAudioService
{
    private AudioSource source;

    public AudioService(AudioSource source)
    {
        this.source = source;
    }

    public void SetVolume(float value)
    {
        source.volume = value;
    }
}