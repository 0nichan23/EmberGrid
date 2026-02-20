using UnityEngine.Events;

[System.Serializable]
public class DamageDealer
{
    public UnityEvent<DamageHandler, DamageDealer, Damageable> OnDealDamage;
    public UnityEvent<UnitAction, DamageDealer, Damageable> OnHit;
    public UnityEvent OnKill;

    private Unit owner;
    public DamageDealer(Unit owner)
    {
        this.owner = owner;
        OnDealDamage = new UnityEvent<DamageHandler, DamageDealer, Damageable>();
        OnHit = new UnityEvent<UnitAction, DamageDealer, Damageable>();
        OnKill = new UnityEvent();
    }

}
