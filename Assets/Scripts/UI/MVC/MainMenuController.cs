using AdvancedRPG.Core;
using AdvancedRPG.Services;
using UnityEngine;

namespace AdvancedRPG.UI.MVC
{
    
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;
        [SerializeField] private string gameSceneName = "Game";

        private void Awake()
        {
            if (view == null)
                view = GetComponent<MainMenuView>();

            view.PlayClicked += LoadGameScene;
            view.ExitClicked += ExitGame;
        }

        private void OnDestroy()
        {
            if (view == null)
                return;

            view.PlayClicked -= LoadGameScene;
            view.ExitClicked -= ExitGame;
        }

        private void LoadGameScene()
        {
            ServiceLocator.Get<ISceneLoader>().Load(gameSceneName);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
