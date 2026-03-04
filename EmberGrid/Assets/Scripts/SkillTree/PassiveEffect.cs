using UnityEngine;

public abstract class PassiveEffect : ScriptableObject
{
    public abstract void Apply(Unit target);
    public abstract void Remove(Unit target);
}
