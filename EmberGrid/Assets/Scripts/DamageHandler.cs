using System.Collections.Generic;
using UnityEngine;

public class DamageHandler
{
    private DamageType damageType;
    private int baseAmount;

    private List<float> mods = new List<float>();
    private List<int> flatMods = new List<int>();

    public DamageHandler(DamageType type, int baseDamage)
    {
        this.damageType = type;
        this.baseAmount = baseDamage;
    }

    public void AddMod(float mod)
    {
        mods.Add(mod);
    }

    public void AddModFlat(int mod)
    {
        if (mod == 0)
            return;


        flatMods.Add(mod);
    }

    public int GetFinalDamage()
    {
        float final = baseAmount;
        
        foreach (var mod in flatMods)
        {
            final += mod;
            final = Mathf.Clamp(final, 0, float.MaxValue);
        }

        if (final <= 0)
        {
            return 0;
        }


        foreach (var item in mods)
        {
            if (item == 0f)
            {
                return 0;
            }
            else if (item < 1f)
            {
                final -= baseAmount * (1 - item);
            }
            else
            {
                final += baseAmount * (item - 1);
            }
        }
        return Mathf.RoundToInt(Mathf.Clamp(final, 0, float.MaxValue));
    }
}
public enum DamageType
{
    Piercing,
    Slashing,
    Bludgeoning,
    Fire,
    Cold,
    Electric,
    Acid,
    Poison,
    Necrotic,
    Radiant
}