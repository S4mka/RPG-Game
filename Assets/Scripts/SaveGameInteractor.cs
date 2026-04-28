using UnityEngine;

public class SaveGameInteractor
{
    private ISaveRepository repository;

    public SaveGameInteractor(ISaveRepository repo)
    {
        repository = repo;
    }

    public void Save(PlayerModel player)
    {
        var data = new SaveData
        {
            hp = player.HP,
            mp = player.MP,
            x = player.Position.x,
            y = player.Position.y,
            z = player.Position.z
        };

        repository.Save(data);
    }

    public void Load(PlayerModel player)
    {
        var data = repository.Load();

        player.Restore(data.hp, data.mp);
        player.Position = new Vector3(data.x, data.y, data.z);
    }
}