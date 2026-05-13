using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        ServiceLocator.Register<IAudioService>(new AudioService(audioSource));
        ServiceLocator.Register<ISaveService>(new SaveService());
    }
}