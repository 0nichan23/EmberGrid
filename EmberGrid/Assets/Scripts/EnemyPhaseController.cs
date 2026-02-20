using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyPhaseController : MonoBehaviour
{
    private Enemy[] cachedEnemies;
    private int currentEnemy = 0;

    private void Start()
    {
        GameManager.Instance.TurnManager.OnEnemyPhase.AddListener(StartEnemyPhase);
    }

    private void OnDisable()
    {
        GameManager.Instance.TurnManager.OnEnemyPhase.RemoveListener(StartEnemyPhase);
    }

    private void StartEnemyPhase()
    {
        cachedEnemies = GameManager.Instance.EnemiesManager.Team.Units.OfType<Enemy>()
            .Where(e => e.Damageable.CurrentHealth > 0)
            .ToArray();
        foreach (Enemy enemy in cachedEnemies)
        {
            enemy.ActionHandler.BeginPhase();
        }
        currentEnemy = 0;
        NextEnemyTurn();
    }

    private void NextEnemyTurn()
    {
        if (cachedEnemies == null || currentEnemy >= cachedEnemies.Length)
        {
            return;
        }
        StartCoroutine(StartEnemyTurn());
    }

    private IEnumerator StartEnemyTurn()
    {
        Debug.Log($"Enemy{currentEnemy} playing turn");
        var enemy = cachedEnemies[currentEnemy];
        currentEnemy++;
        yield return StartCoroutine(enemy.EnemyAI.TakeTurn(NextEnemyTurn));
    }
}
