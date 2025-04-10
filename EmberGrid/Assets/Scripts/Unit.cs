using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private Weapon testWeapon;

    public DamageDealer Dealer { get => dealer; }
    public Damageable Damageable { get => damageable; }
    public UnitStats Stats { get => stats; }
    public BaseStats BaseStats { get => baseStats; }
    public WeaponHandler WeaponHandler { get => weaponHandler; }
    public UnitMovement Movement { get => movement; }
    public UnitSelector Selector { get => selector; }
    public UnitActionHandler ActionHandler { get => actionHandler; }

    protected virtual void Start()
    {
        stats = new UnitStats(baseStats);
        damageable = new Damageable(this, baseStats.MaxHealth);
        weaponHandler = new WeaponHandler(this, testWeapon);
        movement = new UnitMovement(this, 4);
        actionHandler = new UnitActionHandler(this);
        Events();
    }

    protected virtual void Events()
    {
        OnTurnEnded.AddListener(() => GameManager.Instance.SelectionManager.SelectUnit(null));
        OnSelected.AddListener(movement.SetReachableTiles);
    }

    [ContextMenu("TestAttack")]
    public void TestAttack()
    {
        WeaponHandler.Attack(weaponHandler.Weapon.BasicAttack);
    }


    [ContextMenu("TestWait")]
    public void TestWait()
    {
        actionHandler.TakeWaitAction();
    }

}
