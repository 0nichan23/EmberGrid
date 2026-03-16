using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private StatusEffectConfig config;

    public StatusEffectConfig Config => config;

    /// <summary>
    /// Fired per-unit at the start of that unit's phase.
    /// Status instances subscribe to this to apply phase-start effects.
    /// </summary>
    public static event System.Action<Unit> OnUnitPhaseStart;

    /// <summary>
    /// Fired per-unit at the end of that unit's phase.
    /// Status instances subscribe to this to apply phase-end effects (DOTs, decay, removal).
    /// </summary>
    public static event System.Action<Unit> OnUnitPhaseEnd;

    private void Start()
    {
        var tm = GameManager.Instance.TurnManager;
        tm.OnPlayerPhase.AddListener(OnPlayerPhaseStarted);
        tm.OnEnemyPhase.AddListener(OnEnemyPhaseStarted);
    }

    /// <summary>
    /// Player phase just started = enemy phase just ended.
    /// 1. End-of-phase for enemies (their phase ended)
    /// 2. Start-of-phase for heroes (their phase begins)
    /// </summary>
    private void OnPlayerPhaseStarted()
    {
        foreach (var enemy in GameManager.Instance.EnemiesManager.Team.Units)
        {
            OnUnitPhaseEnd?.Invoke(enemy);
        }

        foreach (var hero in GameManager.Instance.PlayerManager.Team.Units)
        {
            OnUnitPhaseStart?.Invoke(hero);
        }
    }

    /// <summary>
    /// Enemy phase just started = player phase just ended.
    /// 1. End-of-phase for heroes (their phase ended)
    /// 2. Start-of-phase for enemies (their phase begins)
    /// </summary>
    private void OnEnemyPhaseStarted()
    {
        foreach (var hero in GameManager.Instance.PlayerManager.Team.Units)
        {
            OnUnitPhaseEnd?.Invoke(hero);
        }

        foreach (var enemy in GameManager.Instance.EnemiesManager.Team.Units)
        {
            OnUnitPhaseStart?.Invoke(enemy);
        }
    }

    /// <summary>
    /// Factory: creates the correct StatusEffectInstance for a given type.
    /// </summary>
    public StatusEffectInstance CreateInstance(Unit owner, StatusEffects type, int stacks)
    {
        switch (type)
        {
            case StatusEffects.Poison: return new PoisonInstance(owner, type, stacks, config);
            case StatusEffects.Burn:   return new BurnInstance(owner, type, stacks, config);
            case StatusEffects.Bleed:  return new BleedInstance(owner, type, stacks, config);
            case StatusEffects.Chill:  return new ChillInstance(owner, type, stacks, config);
            case StatusEffects.Shock:  return new ShockInstance(owner, type, stacks, config);
            case StatusEffects.Stun:   return new StunInstance(owner, type, stacks, config);
            case StatusEffects.Regen:  return new RegenInstance(owner, type, stacks, config);
            case StatusEffects.Haste:  return new HasteInstance(owner, type, stacks, config);
            case StatusEffects.Stride: return new StrideInstance(owner, type, stacks, config);
            default:
                Debug.LogWarning($"No StatusEffectInstance for type {type}");
                return null;
        }
    }
}
