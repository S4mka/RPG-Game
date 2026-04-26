using UnityEngine;

public interface ISaveService
{
    void Save(Vector3 pos);
    Vector3 Load();
}