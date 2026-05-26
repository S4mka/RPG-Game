using System;
using System.Collections.Generic;
using AdvancedRPG.Audio;
using AdvancedRPG.Core;
using AdvancedRPG.Gameplay.Boss;
using AdvancedRPG.Gameplay.Combat;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies
{
    public sealed class MobKillCounter : MonoBehaviour
    {
        [Header("Lab 7 Events")]
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private Transform bossSpawnPoint;
        [SerializeField] private int killsToSpawnBoss = 3;
        [SerializeField] private int killsToPlayVictory = 5;

        [Header("Auto Register")]
        [SerializeField] private bool autoRegisterOnStart = true;
        [SerializeField] private bool ignorePlayer = true;

        private readonly HashSet<CharacterHealthView> registered = new HashSet<CharacterHealthView>();
        private bool bossSpawned;
        private bool victoryPlayed;

        public int Count { get; private set; }
        public event Action<int> Changed;

        private void Start()
        {
            if (autoRegisterOnStart)
                RegisterSceneMobs();

            Changed?.Invoke(Count);
        }

        private void OnDestroy()
        {
            foreach (CharacterHealthView healthView in registered)
            {
                if (healthView != null)
                    healthView.Died -= OnCharacterDied;
            }

            registered.Clear();
        }

        public void RegisterSceneMobs()
        {
            CharacterHealthView[] healthViews = FindObjectsOfType<CharacterHealthView>(true);
            foreach (CharacterHealthView healthView in healthViews)
                Register(healthView);
        }

        public void Register(CharacterHealthView healthView)
        {
            if (healthView == null)
                return;

            if (ignorePlayer && healthView.CompareTag("Player"))
                return;

            if (healthView.GetComponent<BossBrain>() != null)
                return;

            if (!registered.Add(healthView))
                return;

            healthView.Died += OnCharacterDied;
        }

        public void AddKill()
        {
            Count++;
            Changed?.Invoke(Count);
            ProcessMilestones();
        }

        public void SetCount(int count)
        {
            Count = Mathf.Max(0, count);
            Changed?.Invoke(Count);
            ProcessMilestones();
        }

        private void OnCharacterDied(CharacterHealthView deadCharacter)
        {
            if (deadCharacter != null)
                deadCharacter.Died -= OnCharacterDied;

            registered.Remove(deadCharacter);
            AddKill();
        }

        private void ProcessMilestones()
        {
            if (!bossSpawned && Count >= killsToSpawnBoss)
            {
                bossSpawned = true;
                SpawnBoss();
            }

            if (!victoryPlayed && Count >= killsToPlayVictory)
            {
                victoryPlayed = true;
                PlayVictoryMusic();
            }
        }

        private void SpawnBoss()
        {
            if (bossPrefab == null)
                return;

            if (FindObjectOfType<BossBrain>() != null)
                return;

            Vector3 position = bossSpawnPoint == null ? transform.position : bossSpawnPoint.position;
            Quaternion rotation = bossSpawnPoint == null ? Quaternion.identity : bossSpawnPoint.rotation;
            Instantiate(bossPrefab, position, rotation);
        }

        private void PlayVictoryMusic()
        {
            if (ServiceLocator.TryGet<IAudioService>(out var audio))
                audio.PlayVictory();
        }
    }
}
