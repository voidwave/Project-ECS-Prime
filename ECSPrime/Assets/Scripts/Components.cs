using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public struct PlayerInput : IComponentData
{
    public float3 MoveDir;
    public float3 MousePosition;
    //public float FireCooldown;
    //public bool Fire;
}

public struct BotAI : IComponentData
{
    public float3 MoveDir;
    public float3 MousePosition;
    public int targetIndex;
    //public float FireCooldown;
    //public bool Fire;
}

public struct UnitStats : IComponentData
{
    public int team;
    public float Health;
    public Stat MaxHealth, Power, DamageReduction, MovementSpeed;


}
public struct Stat
{
    public float BaseValue;
    private float _value;
    private int isDirty;
    //public NativeArray<StatModifier> StatModifiers;

    public float Value
    {
        get
        {
            if (isDirty == 1)
                CalculateValue();
            return _value;
        }
    }

    public Stat(float baseValue)
    {
        isDirty = 0;
        BaseValue = baseValue;
        _value = BaseValue;
        //StatModifiers = new NativeArray<StatModifier>(10, Allocator.Persistent);
    }

    //public void AddModifier(StatModifier mod)
    //{
    //StatModifiers.Add(mod);
    //isDirty = 1;
    //}

    public void RemoveAllModifiers()
    {
        //StatModifiers.Clear();
        isDirty = 1;
    }
    void CalculateValue()
    {
        _value = BaseValue;

        // for (int i = 0; i < StatModifiers.Length; i++)
        //     if (StatModifiers[i].Type == ModType.Flat)
        //         _value += StatModifiers[i].Value;

        // for (int i = 0; i < StatModifiers.Length; i++)
        //     if (StatModifiers[i].Type == ModType.Percent)
        //         _value *= 1 + StatModifiers[i].Value / 100.0f;

        isDirty = 0;
    }

}

public struct StatModifier
{
    public float Value;
    public ModType Type;
    public int HasDuration;
    public float Duration;

}

public enum ModType { Flat, Percent }