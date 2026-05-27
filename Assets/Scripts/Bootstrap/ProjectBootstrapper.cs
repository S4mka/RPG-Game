using UnityEngine;

namespace AdvancedRPG.Bootstrap
{
    public sealed class ProjectBootstrapper : MonoBehaviour
    {
        private static ProjectBootstrapper instance;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip victoryClip;

        internal static void ResetInstance()
        {
            instance = null;
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            ProjectServices.RegisterDefaults(musicSource, victoryClip, true);
        }
    }
}
