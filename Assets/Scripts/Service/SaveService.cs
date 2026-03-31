using UnityEngine;

public class SaveService : ISaveService
{
    public void Save(Vector3 pos)
    {
        PlayerPrefs.SetFloat("x", pos.x);
        PlayerPrefs.SetFloat("y", pos.y);
        PlayerPrefs.SetFloat("z", pos.z);
    }

    public Vector3 Load()
    {
        return new Vector3(
            PlayerPrefs.GetFloat("x"),
            PlayerPrefs.GetFloat("y"),
            PlayerPrefs.GetFloat("z")
        );
    }
}