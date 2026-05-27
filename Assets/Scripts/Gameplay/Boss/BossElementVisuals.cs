using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossElementVisuals : MonoBehaviour
    {
        [Header("All Boss Melee Weapons")]
        [SerializeField] private GameObject[] allMeleeWeapons;

        [Header("Additional Renderers")]
        [SerializeField] private Renderer[] renderers;

        [Header("Particles")]
        [SerializeField] private ParticleSystem[] particles;

        [Header("Light")]
        [SerializeField] private Light elementLight;

        [Header("Settings")]
        [SerializeField] private bool deactivateOtherWeapons = true;
        [SerializeField] private bool activateSelectedWeapon = true;
        [SerializeField] private bool colorSelectedWeapon = true;
        [SerializeField] private bool colorAdditionalRenderers = true;
        [SerializeField] private bool playParticlesOnApply = true;

        public void Apply(BossLoadout loadout)
        {
            if (loadout == null)
                return;

            Color color = loadout.ElementColor;

            ApplyWeapon(loadout, color);
            ApplyAdditionalRenderers(color);
            ApplyParticles(color);
            ApplyLight(color);
        }

        private void ApplyWeapon(BossLoadout loadout, Color color)
        {
            if (deactivateOtherWeapons)
            {
                DisableAllWeapons();
            }

            if (loadout.WeaponType != BossWeaponType.Melee)
                return;

            if (loadout.MeleeWeaponObject == null)
            {
                Debug.LogWarning($"{name}: BossLoadout '{loadout.Name}' has no melee weapon object.");
                return;
            }

            if (activateSelectedWeapon)
            {
                loadout.MeleeWeaponObject.SetActive(true);
            }

            if (!colorSelectedWeapon)
                return;

            Renderer[] weaponRenderers =
                loadout.MeleeWeaponObject.GetComponentsInChildren<Renderer>(true);

            foreach (Renderer weaponRenderer in weaponRenderers)
            {
                if (weaponRenderer == null)
                    continue;

                weaponRenderer.gameObject.SetActive(true);
                weaponRenderer.enabled = true;

                ApplyColorToRenderer(weaponRenderer, color);
            }
        }

        private void DisableAllWeapons()
        {
            if (allMeleeWeapons == null)
                return;

            foreach (GameObject weapon in allMeleeWeapons)
            {
                if (weapon == null)
                    continue;

                weapon.SetActive(false);
            }
        }

        private void ApplyAdditionalRenderers(Color color)
        {
            if (!colorAdditionalRenderers)
                return;

            if (renderers == null)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (renderer == null)
                    continue;

                if (IsWeaponRenderer(renderer))
                    continue;

                renderer.enabled = true;

                ApplyColorToRenderer(renderer, color);
            }
        }


        private bool IsWeaponRenderer(Renderer renderer)
        {
            if (allMeleeWeapons == null)
                return false;

            foreach (GameObject weapon in allMeleeWeapons)
            {
                if (weapon == null)
                    continue;

                if (renderer.transform == weapon.transform || renderer.transform.IsChildOf(weapon.transform))
                    return true;
            }

            return false;
        }

        private void ApplyParticles(Color color)
        {
            if (particles == null)
                return;

            foreach (ParticleSystem particle in particles)
            {
                if (particle == null)
                    continue;

                particle.gameObject.SetActive(true);

                ParticleSystem.MainModule main = particle.main;
                main.startColor = color;

                if (playParticlesOnApply)
                    particle.Play();
            }
        }

        private void ApplyLight(Color color)
        {
            if (elementLight == null)
                return;

            elementLight.gameObject.SetActive(true);
            elementLight.enabled = true;
            elementLight.color = color;
        }

        private void ApplyColorToRenderer(Renderer renderer, Color color)
        {
            Material[] materials = renderer.materials;

            foreach (Material material in materials)
            {
                if (material == null)
                    continue;

                if (material.HasProperty("_Color"))
                {
                    material.color = color;
                }

                if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor("_BaseColor", color);
                }

                if (material.HasProperty("_EmissionColor"))
                {
                    material.EnableKeyword("_EMISSION");
                    material.SetColor("_EmissionColor", color);
                }
            }
        }
    }
}
