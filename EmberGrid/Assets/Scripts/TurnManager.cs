using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    public UnityEvent OnPlayerPhase;
    public UnityEvent OnEnemyPhase;

    private Phase phase;
    private bool isSwitching;

    public Phase CurrentPhase => phase;

    private void Start()
    {
        phase = Phase.Player;
        OnPlayerPhase?.Invoke();
    }

    [ContextMenu("Phase Switch")]
    public void SwitchPhase()
    {
        if (isSwitching) return;
        isSwitching = true;

        if (phase == Phase.Player)
        {
            phase = Phase.Enemy;
            OnEnemyPhase?.Invoke();
        }
        else
        {
            phase = Phase.Player;
            OnPlayerPhase?.Invoke();
        }
        Debug.Log("switched into " + phase);
        isSwitching = false;
    }
}

public enum Phase
{
    Player,
    Enemy
}