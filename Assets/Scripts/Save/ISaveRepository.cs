namespace AdvancedRPG.Save
{
    public interface ISaveRepository
    {
        void Save(GameSaveData data);
        bool TryLoad(out GameSaveData data);
    }
}
