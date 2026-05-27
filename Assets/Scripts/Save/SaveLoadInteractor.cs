using System.Linq;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies;
using AdvancedRPG.Gameplay.Player;
using UnityEngine;

namespace AdvancedRPG.Save
{
    public sealed class SaveLoadInteractor
    {
        private readonly ISaveRepository repository;
        public SaveLoadInteractor(ISaveRepository repository) => this.repository = repository;

        public void SaveGame(CharacterHealthView player, int killedMobs)
        {
            player = ResolvePlayer(player);
            if (player == null)
            {
                Debug.LogWarning("Save failed: player was not found.");
                return;
            }

            var position = player.transform.position;
            var data = new GameSaveData { playerX = position.x, playerY = position.y, playerZ = position.z, playerHp = player.Health.Current, playerMaxHp = player.Health.Max, killedMobs = killedMobs };
            foreach (var mob in Object.FindObjectsOfType<SavableMob>()) data.mobs.Add(mob.Capture());
            repository.Save(data);
        }

        public bool LoadGame(CharacterHealthView player, MobKillCounter killCounter)
        {
            if (!repository.TryLoad(out var data)) return false;
            player = ResolvePlayer(player);
            if (player == null)
            {
                Debug.LogWarning("Load failed: player was not found.");
                return false;
            }

            TeleportPlayer(player, new Vector3(data.playerX, data.playerY, data.playerZ));
            player.SetHp(data.playerHp, data.playerMaxHp);
            if (killCounter != null) killCounter.SetCount(data.killedMobs);
            var mobs = Object.FindObjectsOfType<SavableMob>().ToDictionary(m => m.Id, m => m);
            foreach (var savedMob in data.mobs) if (mobs.TryGetValue(savedMob.id, out var mob)) mob.Restore(savedMob);
            return true;
        }

        private static CharacterHealthView ResolvePlayer(CharacterHealthView player)
        {
            if (player != null)
                return player;

            var playerObject = GameObject.FindGameObjectWithTag("Player");
            return playerObject == null ? null : playerObject.GetComponent<CharacterHealthView>();
        }

        private static void TeleportPlayer(CharacterHealthView player, Vector3 position)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.Teleport(position);
                return;
            }

            var characterController = player.GetComponent<CharacterController>();
            if (characterController == null)
            {
                player.transform.position = position;
                return;
            }

            characterController.enabled = false;
            player.transform.position = position;
            characterController.enabled = true;
        }
    }
}
