using System;
using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies
{
    public sealed class MobKillCounter : MonoBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private Transform bossSpawnPoint;
        public int Count { get; private set; }
        public event Action<int> Changed;
        public void AddKill()
        {
            Count++;
            Changed?.Invoke(Count);
            if (Count == 3 && bossPrefab != null) Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            if (Count == 5 && ServiceLocator.TryGet<IAudioService>(out var audio)) audio.PlayVictory();
        }
        public void SetCount(int count) { Count = count; Changed?.Invoke(Count); }
    }
}
