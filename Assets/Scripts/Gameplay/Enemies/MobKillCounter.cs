using System;
using System.Collections.Generic;
using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Combat;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies
{
    public sealed class MobKillCounter : MonoBehaviour
    {
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private Transform bossSpawnPoint;
        [SerializeField] private int killsToSpawnBoss = 3;
        [SerializeField] private int killsToPlayVictory = 5;

        private readonly HashSet<CharacterHealthView> trackedEnemies = new HashSet<CharacterHealthView>();
        private readonly HashSet<CharacterHealthView> countedEnemies = new HashSet<CharacterHealthView>();
        private bool bossSpawned;
        private bool victoryPlayed;

        public int Count { get; private set; }
        public event Action<int> Changed;

        private void OnEnable()
        {
            TrackSceneEnemies();
        }

        private void Start()
        {
            TrackSceneEnemies();
            TrySpawnBoss();
        }

        private void OnDisable()
        {
            foreach (CharacterHealthView enemyHealth in trackedEnemies)
                enemyHealth.Died -= OnEnemyDied;

            trackedEnemies.Clear();
        }

        public void AddKill()
        {
            Count++;
            Changed?.Invoke(Count);
            TrySpawnBoss();
            TryPlayVictory();
        }

        public void SetCount(int count)
        {
            Count = count;
            Changed?.Invoke(Count);
            TrySpawnBoss();
            TryPlayVictory();
        }

        public void RegisterEnemy(EnemyBrain enemy)
        {
            if (enemy == null || enemy.HealthView == null || trackedEnemies.Contains(enemy.HealthView))
                return;

            trackedEnemies.Add(enemy.HealthView);
            enemy.HealthView.Died += OnEnemyDied;
        }

        private void TrackSceneEnemies()
        {
            foreach (EnemyBrain enemy in FindObjectsOfType<EnemyBrain>())
                RegisterEnemy(enemy);
        }

        private void OnEnemyDied(CharacterHealthView enemyHealth)
        {
            if (!countedEnemies.Add(enemyHealth))
                return;

            AddKill();
        }

        private void TrySpawnBoss()
        {
            if (bossSpawned || Count < killsToSpawnBoss || bossPrefab == null)
                return;

            Vector3 spawnPosition = bossSpawnPoint != null ? bossSpawnPoint.position : transform.position;
            Quaternion spawnRotation = bossSpawnPoint != null ? bossSpawnPoint.rotation : transform.rotation;

            Instantiate(bossPrefab, spawnPosition, spawnRotation);
            bossSpawned = true;
        }

        private void TryPlayVictory()
        {
            if (victoryPlayed || Count < killsToPlayVictory)
                return;

            if (ServiceLocator.TryGet<IAudioService>(out var audio))
            {
                audio.PlayVictory();
                victoryPlayed = true;
            }
        }
    }
}
