public interface ISaveRepository
{
    void Save(SaveData data);
    SaveData Load();
}