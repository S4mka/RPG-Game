using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject MenuPanel;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        MenuPanel.SetActive(false);

    }

    public void SettingsExit()
    {
        settingsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
}