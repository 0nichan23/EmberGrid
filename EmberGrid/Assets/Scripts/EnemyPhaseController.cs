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
            // All enemies have moved → back to player phase
            GameManager.Instance.TurnManager.SwitchPhase();
            return;
        }

        var enemy = enemies[currentEnemy];
        enemy.ActionHandler.BeginPhase();

        // Kick off its AI coroutine
        StartCoroutine(
            enemy.EnemyAI.TakeTurn(() =>
            {
                // After AI is done, explicitly end its turn
                enemy.ActionHandler.TakeWaitAction();
                currentEnemy++;
                NextEnemyTurn();
            })
        );
    }
}
