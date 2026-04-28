using UnityEngine;

public class GameBootstrapperMB : MonoBehaviour
{
    public static GameBootstrapper Instance;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Instance = new GameBootstrapper();
        Instance.Init();
    }
}