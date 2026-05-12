using UnityEngine;
using UnityEngine.UI;

namespace AdvancedRPG.Gameplay.Combat
{
    /// <summary>
    /// Скрипт вешается на World Space Canvas над персонажем/мобом.
    /// Управляет Slider, который отображает HP объекта с CharacterHealthView.
    /// </summary>
    public sealed class WorldHealthBar : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private CharacterHealthView target;

        [Header("UI")]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private bool hideWhenFull = false;

        [Header("Billboard")]
        [SerializeField] private bool lookAtCamera = true;

        private Camera mainCamera;
        private IHealth subscribedHealth;

        private void Awake()
        {
            if (target == null)
                target = GetComponentInParent<CharacterHealthView>();

            if (hpSlider == null)
                hpSlider = GetComponentInChildren<Slider>(true);

            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            TrySubscribe();
        }

        private void Start()
        {
            // На случай если Canvas включился раньше, чем CharacterHealthView создал HealthModel в Awake.
            TrySubscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void LateUpdate()
        {
            if (!lookAtCamera)
                return;

            if (mainCamera == null)
                mainCamera = Camera.main;

            if (mainCamera != null)
                transform.forward = mainCamera.transform.forward;
        }

        private void TrySubscribe()
        {
            if (subscribedHealth != null)
                return;

            if (target == null)
                target = GetComponentInParent<CharacterHealthView>();

            if (hpSlider == null)
                hpSlider = GetComponentInChildren<Slider>(true);

            if (target == null || target.Health == null || hpSlider == null)
            {
                Debug.LogWarning(
                    $"WorldHealthBar on '{gameObject.name}' is not configured. " +
                    "Put it on a World Space Canvas under an object with CharacterHealthView and assign Slider.",
                    this);
                return;
            }

            subscribedHealth = target.Health;
            subscribedHealth.Changed += UpdateBar;
            UpdateBar(subscribedHealth.Current, subscribedHealth.Max);
        }

        private void Unsubscribe()
        {
            if (subscribedHealth == null)
                return;

            subscribedHealth.Changed -= UpdateBar;
            subscribedHealth = null;
        }

        private void UpdateBar(float current, float max)
        {
            if (hpSlider == null)
                return;

            hpSlider.minValue = 0f;
            hpSlider.maxValue = max;
            hpSlider.value = current;

            if (hideWhenFull)
                hpSlider.gameObject.SetActive(current < max && current > 0f);
        }
    }
}
