using UnityEngine;
using UnityEngine.UI;

#if TMP_PRESENT
using TMPro;
#endif

namespace AdvancedRPG.UI
{
    using AdvancedRPG.Gameplay.Combat;

    public class PlayerHudController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private CharacterHealthView playerHealthView;

        [Header("HP UI")]
        [SerializeField] private Slider hpSlider;

#if TMP_PRESENT
        [SerializeField] private TMP_Text hpText;
#else
        [SerializeField] private Text hpText;
#endif

        [Header("Game Over UI")]
        [SerializeField] private GameObject gameOverPanel;

        private void Awake()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(false);

            if (playerHealthView == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                    playerHealthView = player.GetComponent<CharacterHealthView>();
            }
        }

        private void OnEnable()
        {
            if (playerHealthView == null)
            {
                Debug.LogError($"{nameof(PlayerHudController)}: Player Health View не назначен.", this);
                enabled = false;
                return;
            }

            if (playerHealthView.Health == null)
            {
                Debug.LogError($"{nameof(PlayerHudController)}: у Player Health View не создан HealthModel.", this);
                enabled = false;
                return;
            }

            if (hpSlider == null)
            {
                Debug.LogError($"{nameof(PlayerHudController)}: HP Slider не назначен.", this);
                enabled = false;
                return;
            }

            playerHealthView.Health.Changed += OnHealthChanged;
            playerHealthView.Health.Died += OnPlayerDied;

            OnHealthChanged(
                playerHealthView.Health.Current,
                playerHealthView.Health.Max);
        }

        private void OnDisable()
        {
            if (playerHealthView == null || playerHealthView.Health == null)
                return;

            playerHealthView.Health.Changed -= OnHealthChanged;
            playerHealthView.Health.Died -= OnPlayerDied;
        }

        private void OnHealthChanged(float currentHp, float maxHp)
        {
            hpSlider.minValue = 0;
            hpSlider.maxValue = maxHp;
            hpSlider.value = currentHp;

            if (hpText != null)
                hpText.text = $"HP: {currentHp} / {maxHp}";
        }

        private void OnPlayerDied()
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}