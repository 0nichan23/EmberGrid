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
    private bool isDead;

    public int MaxHealth { get => maxHealth; }
    public int CurrentHealth { get => currentHealth; }
    public bool IsDead { get => isDead; }

    public Damageable(Unit owner, int maxHealth)
    {
        this.owner = owner;
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
        isDead = false;
    }

    public void GetHit(UnitAction action, Unit dealer)
    {
        if (isDead) return;

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
        if (isDead) return;

        OnTakeDamage?.Invoke(handler, dealer.Dealer, this);
        dealer.Dealer.OnDealDamage?.Invoke(handler, dealer.Dealer, this);

        int finalDamage = handler.GetFinalDamage();
        currentHealth -= finalDamage;
        ClampHp();
        Debug.Log($"{owner.name} was hit for {finalDamage} by {dealer.name}");
        if (currentHealth <= 0)
        {
            isDead = true;
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
        int finalDamage = handler.GetFinalDamage();
        Debug.Log($"{owner.name} will be hit for {finalDamage} by {dealer.name}");
    }

    private void ClampHp()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void RestoreDamage(int amount, Unit dealer)
    {
        currentHealth += amount;
        ClampHp();
        OnHeal?.Invoke();
    }

    public void Revive(int healthAmount)
    {
        isDead = false;
        currentHealth = Mathf.Clamp(healthAmount, 1, maxHealth);
    }
}
