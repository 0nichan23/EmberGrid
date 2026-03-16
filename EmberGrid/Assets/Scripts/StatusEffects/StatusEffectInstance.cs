using UnityEngine;

public abstract class StatusEffectInstance
{
    public Unit Owner { get; private set; }
    public StatusEffects Type { get; private set; }
    public int Stacks { get; set; }

    protected StatusEffectConfig Config { get; private set; }
    protected bool IsSubscribed { get; set; }

    public StatusEffectInstance(Unit owner, StatusEffects type, int stacks, StatusEffectConfig config)
    {
        Owner = owner;
        Type = type;
        Stacks = stacks;
        Config = config;
    }

    /// <summary>
    /// Called when the status is first applied. Subscribe to events here.
    /// </summary>
    public abstract void OnApplied();

    /// <summary>
    /// Called when the status is removed (stacks depleted or cleansed). Unsubscribe here.
    /// Must be idempotent — safe to call multiple times.
    /// </summary>
    public abstract void OnRemoved();

    /// <summary>
    /// Decrements stacks by 1. Returns true if stacks reached 0.
    /// </summary>
    public bool Tick()
    {
        Stacks -= 1;
        return Stacks <= 0;
    }

    /// <summary>
    /// Convert to serializable DTO for snapshot/rewind.
    /// </summary>
    public ActiveStatusEffect ToSerializable()
    {
        return new ActiveStatusEffect(Type, Stacks);
    }

    /// <summary>
    /// Helper: ticks and self-removes if depleted.
    /// </summary>
    protected void TickAndRemoveIfDepleted()
    {
        if (Tick())
        {
            Owner.RemoveStatusEffect(Type);
        }
    }
}
