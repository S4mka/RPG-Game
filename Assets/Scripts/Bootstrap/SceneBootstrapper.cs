using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies;
using UnityEngine;

namespace AdvancedRPG.Bootstrap
{
    public sealed class SceneBootstrapper : MonoBehaviour
    {
        [SerializeField] private CharacterHealthView player;
        [SerializeField] private EnemyBrain[] enemies;

        private void Start()
        {
            if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterHealthView>();
            if (enemies == null || enemies.Length == 0) enemies = FindObjectsOfType<EnemyBrain>();
            foreach (var enemy in enemies) enemy.Construct(player.transform);
        }
    }
}
