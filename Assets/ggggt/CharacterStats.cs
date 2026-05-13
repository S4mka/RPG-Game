using UnityEngine;

[CreateAssetMenu(menuName = "RPG/Character Stats")]
public class CharacterStats : ScriptableObject
{
    public float maxHP = 100;
    public float physicalDamage = 10;
    public float magicalDamage = 20;
}