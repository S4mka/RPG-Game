using System.IO;
using UnityEngine;

namespace AdvancedRPG.Save
{
    public sealed class JsonFileSaveRepository : ISaveRepository
    {
        private readonly string path;
        public JsonFileSaveRepository(string fileName) => path = Path.Combine(Application.persistentDataPath, fileName);
        public void Save(GameSaveData data) => File.WriteAllText(path, JsonUtility.ToJson(data, true));
        public bool TryLoad(out GameSaveData data)
        {
            if (!File.Exists(path)) { data = null; return false; }
            data = JsonUtility.FromJson<GameSaveData>(File.ReadAllText(path));
            return data != null;
        }
    }
}
