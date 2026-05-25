using AdvancedRPG.Gameplay.Enemies;
using TMPro;
using UnityEngine;

namespace AdvancedRPG.UI
{
    public sealed class ScoreboardView : MonoBehaviour
    {
        [SerializeField] private MobKillCounter counter;
        [SerializeField] private TMP_Text text;
        private void OnEnable() { counter.Changed += UpdateText; UpdateText(counter.Count); }
        private void OnDisable() { counter.Changed -= UpdateText; }
        private void UpdateText(int count) => text.text = $"Score: {count}";
    }
}
