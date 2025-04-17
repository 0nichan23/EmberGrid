using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerTeam team;

    public PlayerTeam Team { get => team; }

    private void Start()
    {
        team.Setup();
        GameManager.Instance.UIManager.HerosBar.CreateSlots(team.Units);
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(team.UnLockUnits);
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(team.PlayerPhaseReset);
        GameManager.Instance.TurnManager.OnEnemyPhase.AddListener(team.LockUnits);
    }

}
