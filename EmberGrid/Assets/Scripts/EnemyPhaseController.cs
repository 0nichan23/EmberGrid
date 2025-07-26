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
        currentEnemy = 0;
        NextEnemyTurn();
    }

    private void NextEnemyTurn()
    {

        if (currentEnemy >= enemies.Length)
        {
            return;
        }

        var enemy = enemies[currentEnemy];
        enemy.ActionHandler.BeginPhase();
        currentEnemy++;

        // Kick off its AI coroutine
        StartCoroutine(
            enemy.EnemyAI.TakeTurn(() =>
            {
                NextEnemyTurn();
            })
        );
    }
}
