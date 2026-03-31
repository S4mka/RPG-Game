using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuUI : MonoBehaviour
{
    public GameObject panel;
    public Transform player;

    private ISaveService save;

    private void Start()
    {
        save = ServiceLocator.Get<ISaveService>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            panel.SetActive(!panel.activeSelf);
    }

    public void Save()
    {
        save.Save(player.position);
    }

    public void Load()
    {
        player.position = save.Load();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}