public class GameBootstrapper
{
    public ISaveRepository SaveRepository;
    public SaveGameInteractor SaveInteractor;
    public PlayerMover PlayerMover;

    public void Init()
    {
        SaveRepository = new PlayerPrefsSaveRepository();
        SaveInteractor = new SaveGameInteractor(SaveRepository);

        PlayerMover = new PlayerMover(5f, 8f);
    }
}