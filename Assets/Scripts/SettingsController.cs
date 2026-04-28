using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float v)
    {
        AudioListener.volume = v;
    }
}