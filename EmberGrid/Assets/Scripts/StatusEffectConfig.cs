using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusEffectData
{
    public StatusEffects Type;
    public int MaxStacks;
    [Tooltip("% of max HP for Poison/Bleed/Regen, flat damage for Burn, % damage increase per stack for Shock, speed multiplier for Chill (0.5 = halve), speed multiplier for Stride (2 = double). Unused for Stun/Haste.")]
    public float Value;
}

[CreateAssetMenu(menuName = "Config/StatusEffectConfig", fileName = "StatusEffectConfig")]
public class StatusEffectConfig : ScriptableObject
{
    [SerializeField] private StatusEffectData[] effects;

    private Dictionary<StatusEffects, StatusEffectData> lookup;

    private void BuildLookup()
    {
        lookup = new Dictionary<StatusEffects, StatusEffectData>();
        if (effects == null) return;
        foreach (var data in effects)
        {
            lookup[data.Type] = data;
        }
    }

    public StatusEffectData GetData(StatusEffects type)
    {
        if (lookup == null)
            BuildLookup();

        if (lookup.TryGetValue(type, out var data))
            return data;

        Debug.LogWarning($"StatusEffectConfig: No data found for {type}");
        return null;
    }

    public int GetMaxStacks(StatusEffects type)
    {
        var data = GetData(type);
        return data != null ? data.MaxStacks : 1;
    }

    public float GetValue(StatusEffects type)
    {
        var data = GetData(type);
        return data != null ? data.Value : 0f;
    }
}
