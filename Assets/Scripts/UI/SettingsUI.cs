using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnChange);
    }

    void OnChange(float value)
    {
        ServiceLocator.Get<IAudioService>().SetVolume(value);
    }
}