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

    private WeaponHandler primaryHandler;
    private WeaponHandler secondaryHandler;
    private List<UnitAction> utilityActions;
    private List<PassiveEffect> activePassives;

    public DamageDealer Dealer { get => dealer; }
    public Damageable Damageable { get => damageable; }
    public UnitStats Stats { get => stats; }
    public BaseStats BaseStats { get => baseStats; }
    public WeaponHandler WeaponHandler { get => weaponHandler; }
    public WeaponHandler PrimaryHandler { get => primaryHandler; }
    public WeaponHandler SecondaryHandler { get => secondaryHandler; }
    public List<UnitAction> UtilityActions { get => utilityActions; }
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

        // Enemy/legacy path: use testWeapon directly
        if (testWeapon != null)
        {
            weaponHandler = new WeaponHandler(this, testWeapon);
            primaryHandler = weaponHandler;
        }

        int moveSpeed = baseStats.MovementSpeed > 0 ? baseStats.MovementSpeed : 4;
        movement = new UnitMovement(this, moveSpeed);
        actionHandler = new UnitActionHandler(this);
        Events();
    }

    public void InitializeFromLoadout(ResolvedLoadout loadout)
    {
        primaryHandler = new WeaponHandler(this, loadout.PrimaryWeapon,
            loadout.PrimaryAtWill, loadout.PrimaryEncounter, loadout.PrimaryUltimate);

        secondaryHandler = new WeaponHandler(this, loadout.SecondaryWeapon,
            loadout.SecondaryAtWill, loadout.SecondaryEncounter, loadout.SecondaryUltimate);

        // Default weaponHandler to primary for backward compat
        weaponHandler = primaryHandler;

        utilityActions = new List<UnitAction>(loadout.UtilityActions);

        activePassives = new List<PassiveEffect>(loadout.PassiveEffects);
        foreach (var passive in activePassives)
        {
            passive.Apply(this);
        }
    }

    /// <summary>
    /// Returns all available actions across both weapons and utility, filtered by remaining AP.
    /// </summary>
    public List<UnitAction> GetAllAvailableActions(int remainingAP)
    {
        var actions = new List<UnitAction>();

        if (primaryHandler != null)
            actions.AddRange(primaryHandler.GetAvailableActions(remainingAP));
        if (secondaryHandler != null)
            actions.AddRange(secondaryHandler.GetAvailableActions(remainingAP));
        if (utilityActions != null)
        {
            foreach (var action in utilityActions)
            {
                if (action != null && action.Cost <= remainingAP)
                    actions.Add(action);
            }
        }

        return actions;
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
        if (weaponHandler != null && weaponHandler.BasicAttack != null)
            weaponHandler.SetAttackMode(weaponHandler.BasicAttack);
    }


    [ContextMenu("TestWait")]
    public void TestWait()
    {
        actionHandler.TakeWaitAction();
    }
}

public enum ActiveMode
{
    Unselected,
    AttackMode,
    MovementMode,
}
