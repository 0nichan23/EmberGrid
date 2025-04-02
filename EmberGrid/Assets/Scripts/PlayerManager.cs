using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerTeam team;

    private void Start()
    {
        team.Setup();
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(team.UnLockUnits);
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(team.PlayerPhaseReset);
        GameManager.Instance.TurnManager.OnEnemyPhase.AddListener(team.LockUnits);
    }

}
