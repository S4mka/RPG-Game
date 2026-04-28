using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameMenuView view;
    public Transform playerTransform;

    private PlayerModel player;
    private SaveGameInteractor interactor;

    private void Start()
    {
        player = new PlayerModel(100, 50);
        interactor = GameBootstrapperMB.Instance.SaveInteractor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    void Toggle()
    {
        if (view.panel.activeSelf)
        {
            view.Hide();
            Time.timeScale = 1;
        }
        else
        {
            view.Show();
            Time.timeScale = 0;
        }
    }

    public void Save()
    {
        player.Position = playerTransform.position;
        interactor.Save(player);
    }

    public void Load()
    {
        interactor.Load(player);
        playerTransform.position = player.Position;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}