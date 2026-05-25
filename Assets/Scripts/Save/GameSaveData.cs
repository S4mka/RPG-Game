using System;
using System.Collections.Generic;

namespace AdvancedRPG.Save
{
    [Serializable]
    public sealed class GameSaveData
    {
        public float playerX, playerY, playerZ;
        public float playerHp, playerMaxHp;
        public int killedMobs;
        public List<MobSaveData> mobs = new List<MobSaveData>();
    }

    [Serializable]
    public sealed class MobSaveData
    {
        public string id;
        public float x, y, z;
        public float hp, maxHp;
        public bool dead;
    }
}
