using UnityEngine;

public class PlayerPrefsSaveRepository : ISaveRepository
{
    public void Save(SaveData data)
    {
        PlayerPrefs.SetFloat("hp", data.hp);
        PlayerPrefs.SetFloat("mp", data.mp);
        PlayerPrefs.SetFloat("x", data.x);
        PlayerPrefs.SetFloat("y", data.y);
        PlayerPrefs.SetFloat("z", data.z);
    }

    public SaveData Load()
    {
        return new SaveData
        {
            hp = PlayerPrefs.GetFloat("hp", 100),
            mp = PlayerPrefs.GetFloat("mp", 50),
            x = PlayerPrefs.GetFloat("x", 0),
            y = PlayerPrefs.GetFloat("y", 0),
            z = PlayerPrefs.GetFloat("z", 0)
        };
    }
}