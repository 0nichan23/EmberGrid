using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    [SerializeField] private StatusEffectConfig config;

    public StatusEffectConfig Config => config;

    private void Start()
    {
        var tm = GameManager.Instance.TurnManager;
        tm.OnPlayerPhase.AddListener(OnPlayerPhaseStarted);
        tm.OnEnemyPhase.AddListener(OnEnemyPhaseStarted);
    }

    /// <summary>
    /// Player phase just started = enemy phase just ended.
    /// 1. End-of-phase ticks for enemies (their phase ended)
    /// 2. Start-of-phase ticks for heroes (their phase begins)
    /// </summary>
    private void OnPlayerPhaseStarted()
    {
        foreach (var enemy in GameManager.Instance.EnemiesManager.Team.Units)
        {
            StatusEffectProcessor.ProcessPhaseEnd(enemy, config);
        }

        foreach (var hero in GameManager.Instance.PlayerManager.Team.Units)
        {
            StatusEffectProcessor.ProcessPhaseStart(hero, config);
        }
    }

    /// <summary>
    /// Enemy phase just started = player phase just ended.
    /// 1. End-of-phase ticks for heroes (their phase ended)
    /// 2. Start-of-phase ticks for enemies (their phase begins)
    /// </summary>
    private void OnEnemyPhaseStarted()
    {
        foreach (var hero in GameManager.Instance.PlayerManager.Team.Units)
        {
            StatusEffectProcessor.ProcessPhaseEnd(hero, config);
        }

        foreach (var enemy in GameManager.Instance.EnemiesManager.Team.Units)
        {
            StatusEffectProcessor.ProcessPhaseStart(enemy, config);
        }
    }
}
