using AdvancedRPG.Gameplay.Enemies;
using TMPro;
using UnityEngine;

namespace AdvancedRPG.UI
{
    public sealed class ScoreboardView : MonoBehaviour
    {
        [SerializeField] private MobKillCounter counter;
        [SerializeField] private TMP_Text text;
        [SerializeField] private string format = "Score: {0}";

        private void Awake()
        {
            if (counter == null)
                counter = FindObjectOfType<MobKillCounter>();
        }

        private void OnEnable()
        {
            if (counter == null || text == null)
                return;

            counter.Changed += UpdateText;
            UpdateText(counter.Count);
        }

        private void OnDisable()
        {
            if (counter != null)
                counter.Changed -= UpdateText;
        }

        private void UpdateText(int count)
        {
            if (text != null)
                text.text = string.Format(format, count);
        }
    }
}
