using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    //keep track of phase 
    //player phase -> when a unit is selected lock every other unit so that it is not clickable 
    //once a unit has performed an action mark it as a expanded 
    //once player pahse starts unlock all characters 

    public UnityEvent OnPlayerPhase;
    public UnityEvent OnEnemyPhase;

    private Phase phase;

    private void Start()
    {
        phase = Phase.Player;
        OnPlayerPhase?.Invoke();
    }

    [ContextMenu("Phase Switch")]
    public void SwitchPhase()
    {
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
    }



}

public enum Phase
{
    Player,
    Enemy
}