using UnityEngine;

namespace AdvancedRPG.Audio
{
    public sealed class UnityAudioService : IAudioService
    {
        private readonly AudioSource musicSource;
        private readonly AudioClip victoryClip;
        public float MusicVolume { get => musicSource == null ? 1f : musicSource.volume; set { if (musicSource != null) musicSource.volume = Mathf.Clamp01(value); } }
        public UnityAudioService(AudioSource musicSource, AudioClip victoryClip) { this.musicSource = musicSource; this.victoryClip = victoryClip; }
        public void PlayVictory() { if (musicSource != null && victoryClip != null) musicSource.PlayOneShot(victoryClip); }
    }
}
