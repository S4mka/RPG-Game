using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.UI.MVC
{
    public sealed class SettingsView : MonoBehaviour
    {
        [SerializeField] private Slider musicVolumeSlider;
        private void Start()
        {
            var audio = ServiceLocator.Get<IAudioService>();
            musicVolumeSlider.value = audio.MusicVolume;
            musicVolumeSlider.onValueChanged.AddListener(v => audio.MusicVolume = v);
        }
    }
}
