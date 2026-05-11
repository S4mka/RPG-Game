using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies;
using AdvancedRPG.Save;
using AdvancedRPG.Services;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.UI
{
    public sealed class PauseMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private string mainMenuSceneName = "MainMenu";
        [SerializeField] private CharacterHealthView player;
        [SerializeField] private MobKillCounter killCounter;
        private bool opened;
        private void Awake()
        {
            mainMenuButton.onClick.AddListener(() => ServiceLocator.Get<ISceneLoader>().Load(mainMenuSceneName));
            saveButton.onClick.AddListener(() => ServiceLocator.Get<SaveLoadInteractor>().SaveGame(player, killCounter == null ? 0 : killCounter.Count));
            loadButton.onClick.AddListener(() => ServiceLocator.Get<SaveLoadInteractor>().LoadGame(player, killCounter));
            panel.SetActive(false);
        }
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            opened = !opened; panel.SetActive(opened); Time.timeScale = opened ? 0f : 1f;
            Cursor.lockState = opened ? CursorLockMode.None : CursorLockMode.Locked; Cursor.visible = opened;
        }
    }
}
