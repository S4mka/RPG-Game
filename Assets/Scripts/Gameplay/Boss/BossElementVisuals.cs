using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossElementVisuals : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;
        [SerializeField] private ParticleSystem[] particles;
        [SerializeField] private Light elementLight;

        public void Apply(BossLoadout loadout)
        {
            if (loadout == null)
                return;

            Color color = loadout.ElementColor;

            if (renderers != null)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer == null)
                        continue;

                    foreach (Material material in renderer.materials)
                    {
                        if (material != null && material.HasProperty("_Color"))
                            material.color = color;
                    }
                }
            }

            if (particles != null)
            {
                foreach (ParticleSystem particle in particles)
                {
                    if (particle == null)
                        continue;

                    ParticleSystem.MainModule main = particle.main;
                    main.startColor = color;
                }
            }

            if (elementLight != null)
                elementLight.color = color;
        }
    }
}
