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
    [SerializeField] private SpriteRenderer visual; //temp
    [SerializeField] private Weapon testWeapon;
    [SerializeField] private ActiveMode currentMode;

    private List<ActiveStatusEffect> activeEffects = new List<ActiveStatusEffect>();

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

    protected virtual void Start()
    {
        stats = new UnitStats(baseStats);
        damageable = new Damageable(this, baseStats.MaxHealth);
        dealer = new DamageDealer(this);
        weaponHandler = new WeaponHandler(this, testWeapon);
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

    [ContextMenu("TestAttack")]
    public void TestAttack()
    {
        WeaponHandler.SetAttackMode(weaponHandler.Weapon.BasicAttack);
    }


    [ContextMenu("TestWait")]
    public void TestWait()
    {
        actionHandler.TakeWaitAction();
    }

    #region Status Effects

    public void AddStatusEffect(StatusEffects type, int stacks)
    {
        var config = GameManager.Instance.StatusEffectManager.Config;
        int maxStacks = config.GetMaxStacks(type);

        var existing = activeEffects.Find(e => e.Type == type);
        if (existing != null)
        {
            existing.Stacks = Mathf.Min(existing.Stacks + stacks, maxStacks);
        }
        else
        {
            activeEffects.Add(new ActiveStatusEffect(type, Mathf.Min(stacks, maxStacks)));
        }
    }

    public void RemoveStatusEffect(StatusEffects type)
    {
        bool hadStun = type == StatusEffects.Stun && HasStatus(StatusEffects.Stun);
        activeEffects.RemoveAll(e => e.Type == type);

        // If stun was cleansed mid-phase, re-enable the unit
        if (hadStun)
        {
            actionHandler.OnStunCleansed();
        }
    }

    public void TickStatusEffect(StatusEffects type)
    {
        var existing = activeEffects.Find(e => e.Type == type);
        if (existing == null) return;

        existing.Stacks -= 1;
        if (existing.Stacks <= 0)
        {
            activeEffects.Remove(existing);
        }
    }

    public int GetStatusStacks(StatusEffects type)
    {
        var existing = activeEffects.Find(e => e.Type == type);
        return existing != null ? existing.Stacks : 0;
    }

    public bool HasStatus(StatusEffects type)
    {
        return activeEffects.Exists(e => e.Type == type && e.Stacks > 0);
    }

    public List<ActiveStatusEffect> GetActiveEffects()
    {
        return activeEffects;
    }

    public void SetActiveEffects(List<ActiveStatusEffect> effects)
    {
        activeEffects = effects;
    }

    #endregion
}

public enum ActiveMode
{
    Unselected,
    AttackMode,
    MovementMode,
}
