using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Health playerHealth;

    private void Start()
    {
        playerHealth.OnDeath += ShowGameOver;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}