using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.Gameplay.Combat
{
    public sealed class WorldHealthBar : MonoBehaviour
    {
        [SerializeField] private CharacterHealthView target;
        [SerializeField] private Image fill;
        [SerializeField] private Transform billboardRoot;

        private Camera mainCamera;

        private void Awake()
        {
            if (target == null) target = GetComponentInParent<CharacterHealthView>();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            if (target != null) target.Health.Changed += UpdateBar;
        }

        private void OnDisable()
        {
            if (target != null) target.Health.Changed -= UpdateBar;
        }

        private void LateUpdate()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (billboardRoot != null && mainCamera != null)
                billboardRoot.forward = mainCamera.transform.forward;
        }

        private void UpdateBar(float current, float max)
        {
            if (fill != null) fill.fillAmount = max <= 0 ? 0 : current / max;
        }
    }
}
