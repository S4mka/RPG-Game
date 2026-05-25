using System.Linq;
using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies;
using UnityEngine;

namespace AdvancedRPG.Save
{
    public sealed class SaveLoadInteractor
    {
        private readonly ISaveRepository repository;
        public SaveLoadInteractor(ISaveRepository repository) => this.repository = repository;

        public void SaveGame(CharacterHealthView player, int killedMobs)
        {
            var data = new GameSaveData { playerX = player.transform.position.x, playerY = player.transform.position.y, playerZ = player.transform.position.z, playerHp = player.Health.Current, playerMaxHp = player.Health.Max, killedMobs = killedMobs };
            foreach (var mob in Object.FindObjectsOfType<SavableMob>()) data.mobs.Add(mob.Capture());
            repository.Save(data);
        }

        public bool LoadGame(CharacterHealthView player, MobKillCounter killCounter)
        {
            if (!repository.TryLoad(out var data)) return false;
            player.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);
            player.SetHp(data.playerHp, data.playerMaxHp);
            if (killCounter != null) killCounter.SetCount(data.killedMobs);
            var mobs = Object.FindObjectsOfType<SavableMob>().ToDictionary(m => m.Id, m => m);
            foreach (var savedMob in data.mobs) if (mobs.TryGetValue(savedMob.id, out var mob)) mob.Restore(savedMob);
            return true;
        }
    }
}
