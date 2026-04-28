using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject settingsPanel;

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }
}