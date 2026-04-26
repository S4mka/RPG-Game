using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
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
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            panel.SetActive(!panel.activeSelf);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        if (panel.activeSelf == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

       
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
        SceneManager.LoadScene(0);
    }
}