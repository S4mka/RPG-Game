public enum DamageType
{
    Physical,
    Magical
}

public struct DamageData
{
    public float Amount;
    public DamageType Type;

    public DamageData(float amount, DamageType type)
    {
        Amount = amount;
        Type = type;
    }
}