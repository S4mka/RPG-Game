using AdvancedRPG.Gameplay.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.UI
{
    public sealed class PlayerHudController : MonoBehaviour
    {
        [SerializeField] private CharacterHealthView player;
        [SerializeField] private Image hpFill;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private GameObject gameOverPanel;
        private void OnEnable()
        {
            player.Health.Changed += OnHpChanged;
            player.Health.Died += OnDied;
            OnHpChanged(player.Health.Current, player.Health.Max);
        }
        private void OnDisable()
        {
            player.Health.Changed -= OnHpChanged;
            player.Health.Died -= OnDied;
        }
        private void OnHpChanged(float current, float max)
        {
            hpFill.fillAmount = current / max;
            hpText.text = $"HP: {current:0}/{max:0}";
        }
        private void OnDied() { gameOverPanel.SetActive(true); Time.timeScale = 0f; Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    }
}
