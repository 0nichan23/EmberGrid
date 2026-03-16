using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class Unit : MonoBehaviour
{
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;
    public UnityEvent OnTurnEnded;

    [SerializeField] private UnitSelector selector;
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private UnitStats stats;
    [SerializeField] private Damageable damageable;
    [SerializeField] private DamageDealer dealer;
    [SerializeField] private WeaponHandler weaponHandler;
    [SerializeField] private UnitMovement movement;
    [SerializeField] private UnitActionHandler actionHandler;
    [SerializeField] private SpriteRenderer visual;
    [SerializeField] private ActiveMode currentMode;

    private Dictionary<StatusEffects, StatusEffectInstance> activeInstances = new Dictionary<StatusEffects, StatusEffectInstance>();

    public DamageDealer Dealer { get => dealer; }
    public Damageable Damageable { get => damageable; }
    public UnitStats Stats { get => stats; }
    public BaseStats BaseStats { get => baseStats; }
    public WeaponHandler WeaponHandler { get => weaponHandler; }
    public UnitMovement Movement { get => movement; }
    public UnitSelector Selector { get => selector; }
    public UnitActionHandler ActionHandler { get => actionHandler; }
    public SpriteRenderer Visual { get => visual; }
    public ActiveMode CurrentMode { get => currentMode; set => currentMode = value; }

    protected void SetWeaponHandler(WeaponHandler handler)
    {
        weaponHandler = handler;
    }

    protected virtual void Start()
    {
        stats = new UnitStats(baseStats);
        damageable = new Damageable(this, baseStats.MaxHealth);
        dealer = new DamageDealer(this);
        int moveSpeed = baseStats.MovementSpeed > 0 ? baseStats.MovementSpeed : 4;
        movement = new UnitMovement(this, moveSpeed);
        actionHandler = new UnitActionHandler(this);
        Events();
    }

    protected virtual void Events()
    {
        OnTurnEnded.AddListener(() => GameManager.Instance.SelectionManager.SelectUnit(null));
        OnTurnEnded.AddListener(() => currentMode = ActiveMode.Unselected);
        OnSelected.AddListener(() => GameManager.Instance.UIManager.UnitPanel.Setup(this));
        OnDeselected.AddListener(() => currentMode = ActiveMode.Unselected);
       // OnSelected.AddListener(PositionCamOnUnit);
        OnSelected.AddListener(movement.SetMovementMode);
    }


    public void PositionCamOnUnit()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }


    [ContextMenu("TestWait")]
    public void TestWait()
    {
        actionHandler.TakeWaitAction();
    }

    #region Status Effects

    public void AddStatusEffect(StatusEffects type, int stacks)
    {
        var manager = GameManager.Instance.StatusEffectManager;
        var config = manager.Config;
        int maxStacks = config.GetMaxStacks(type);

        if (activeInstances.TryGetValue(type, out var existing))
        {
            existing.Stacks = Mathf.Min(existing.Stacks + stacks, maxStacks);
        }
        else
        {
            var instance = manager.CreateInstance(this, type, Mathf.Min(stacks, maxStacks));
            if (instance != null)
            {
                activeInstances[type] = instance;
                instance.OnApplied();
            }
        }
    }

    public void RemoveStatusEffect(StatusEffects type)
    {
        if (!activeInstances.TryGetValue(type, out var instance)) return;

        bool hadStun = type == StatusEffects.Stun;

        instance.OnRemoved();
        activeInstances.Remove(type);

        // If stun was cleansed mid-phase, re-enable the unit
        if (hadStun)
        {
            actionHandler.OnStunCleansed();
        }
    }

    public int GetStatusStacks(StatusEffects type)
    {
        return activeInstances.TryGetValue(type, out var instance) ? instance.Stacks : 0;
    }

    public bool HasStatus(StatusEffects type)
    {
        return activeInstances.TryGetValue(type, out var instance) && instance.Stacks > 0;
    }

    /// <summary>
    /// Builds a serializable list from the active instances (for snapshots/rewind).
    /// </summary>
    public List<ActiveStatusEffect> GetActiveEffects()
    {
        var list = new List<ActiveStatusEffect>();
        foreach (var kvp in activeInstances)
        {
            list.Add(kvp.Value.ToSerializable());
        }
        return list;
    }

    /// <summary>
    /// Tears down all current instances and rebuilds from a serializable list (for rewind/restore).
    /// </summary>
    public void SetActiveEffects(List<ActiveStatusEffect> effects)
    {
        // Remove all current instances (unsubscribe from events)
        foreach (var kvp in activeInstances)
        {
            kvp.Value.OnRemoved();
        }
        activeInstances.Clear();

        // Rebuild from serializable data
        if (effects == null) return;
        foreach (var effect in effects)
        {
            AddStatusEffect(effect.Type, effect.Stacks);
        }
    }

    #endregion
}

public enum ActiveMode
{
    Unselected,
    AttackMode,
    MovementMode,
}
