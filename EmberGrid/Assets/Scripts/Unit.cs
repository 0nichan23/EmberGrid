using System.Runtime.CompilerServices;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private UnitStats stats;
    [SerializeField] private Damageable damageable;
    [SerializeField] private DamageDealer dealer;


    [SerializeField] private Unit testTarget;
    [SerializeField] private UnitAction testAction;

    public DamageDealer Dealer { get => dealer; }
    public Damageable Damageable { get => damageable; }
    public UnitStats Stats { get => stats; }
    public BaseStats BaseStats { get => baseStats; }

    private void Start()
    {
        stats = new UnitStats(baseStats);
        damageable = new Damageable(this, baseStats.MaxHealth);
        TestFireDamageIncrease();
    }

    [ContextMenu("Test attacking")]
    public void TestAttack()
    {
        testTarget.damageable.GetHit(testAction, this);
    }

    public void TestFireDamageIncrease()
    {
        dealer.OnDealDamage.AddListener(AddFireDamage);
    }

    private void AddFireDamage(DamageHandler handler, DamageDealer delaer, Damageable target)
    {
        handler.AddMod(1.75f);
    }

}
