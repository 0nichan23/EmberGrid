using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Damageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    public UnityEvent<DamageHandler, DamageDealer, Damageable> OnTakeDamage;
    public UnityEvent<UnitAction, DamageDealer, Damageable> OnGetHit;
    public UnityEvent OnHeal;
    public UnityEvent OnDeath;

    private Unit owner;

    public int MaxHealth { get => maxHealth; }
    public int CurrentHealth { get => currentHealth; }

    public Damageable(Unit owner, int maxHealth)
    {
        this.owner = owner;
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
    }

    public void GetHit(UnitAction action, Unit dealer)
    {
        //accuracy calc? 
        OnGetHit?.Invoke(action, dealer.Dealer, this);
        dealer.Dealer.OnHit?.Invoke(action, dealer.Dealer, this);

        foreach (var item in action.Effects)
        {
            item.InvokeEffect(owner, dealer);
        }
    }

    public void TakeDamage(DamageHandler handler, Unit dealer)
    {
        OnTakeDamage?.Invoke(handler, dealer.Dealer, this);
        dealer.Dealer.OnDealDamage?.Invoke(handler, dealer.Dealer, this);

        int finalDamge = handler.GetFinalDamage();
        currentHealth -= finalDamge;
        Debug.Log($"{owner.name} was hit for {finalDamge} by {dealer.name}");
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            dealer.Dealer.OnKill?.Invoke();
        }
    }

    public void GetHitDisplay(UnitAction action, Unit dealer)
    {
        //does not trigger events 
        //invokes a display effect only  
        foreach (var item in action.Effects)
        {
            item.InvokeDisplayEffect(owner, dealer);
        }
    }

    public void TakeDamageDisplay(DamageHandler handler, Unit dealer)
    {
        int finalDamge = handler.GetFinalDamage();
        Debug.Log($"{owner.name} will be hit for {finalDamge} by {dealer.name}");
    }

    private void ClampHp()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }


    public void RestoreDamage(int amount, Unit dealer)
    {
        OnHeal?.Invoke();
    }


}
