using UnityEngine;

public class GameBootstrapperMB : MonoBehaviour
{
    public static GameBootstrapperMB Instance { get; private set; }

    public PlayerMover PlayerMover { get; private set; }
    // Добавь эту строку:
    public SaveInteractor SaveInteractor { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayerMover = new PlayerMover();
        // Инициализируй здесь, если у тебя есть такой класс:
        SaveInteractor = new SaveInteractor();
    }
}