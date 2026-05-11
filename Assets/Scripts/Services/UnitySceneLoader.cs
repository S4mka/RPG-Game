using UnityEngine.SceneManagement;

namespace AdvancedRPG.Services
{
    public sealed class UnitySceneLoader : ISceneLoader
    {
        public void Load(string sceneName) => SceneManager.LoadScene(sceneName);
        public void ReloadCurrent() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
