using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyPhaseController : MonoBehaviour
{
    private Enemy[] enemies => GameManager.Instance.EnemiesManager.Team.Units.OfType<Enemy>().ToArray();
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
        foreach (Enemy enemy in enemies)
        {
            enemy.ActionHandler.BeginPhase();
        }
        currentEnemy = 0;
        NextEnemyTurn();
    }

    private void NextEnemyTurn()
    {
        if (currentEnemy >= enemies.Length)
        {
            return;
        }
        StartCoroutine(StartEnemyTurn());
    }

    private IEnumerator StartEnemyTurn()
    {
        Debug.Log($"Enemy{currentEnemy} playing turn");
        var enemy = enemies[currentEnemy];
        currentEnemy++;
        yield return StartCoroutine(enemy.EnemyAI.TakeTurn(NextEnemyTurn));

    }
}
