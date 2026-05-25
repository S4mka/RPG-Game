namespace AdvancedRPG.Services
{
    public interface ISceneLoader
    {
        void Load(string sceneName);
        void ReloadCurrent();
    }
}
