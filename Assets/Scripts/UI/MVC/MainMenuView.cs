using System;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.UI.MVC
{
    
    public sealed class MainMenuView : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("Main menu buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;

        [Header("Settings buttons")]
        [SerializeField] private Button backToMainMenuButton;

        public event Action PlayClicked;
        public event Action ExitClicked;

        private void Awake()
        {
            playButton.onClick.AddListener(() => PlayClicked?.Invoke());
            settingsButton.onClick.AddListener(ShowSettingsPanel);
            exitButton.onClick.AddListener(() => ExitClicked?.Invoke());
            backToMainMenuButton.onClick.AddListener(ShowMainMenuPanel);

            ShowMainMenuPanel();
        }

        public void ShowMainMenuPanel()
        {
            mainMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }

        public void ShowSettingsPanel()
        {
            mainMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
        }
    }
}
