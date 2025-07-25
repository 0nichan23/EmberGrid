using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private UnitTeam playerTeam;

    public UnitTeam Team { get => playerTeam; }

    private void Start()
    {
        playerTeam.Setup();
        GameManager.Instance.UIManager.HerosBar.CreateSlots(playerTeam.Units);
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(playerTeam.UnLockUnits);
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(playerTeam.PlayerPhaseReset);
        GameManager.Instance.TurnManager.OnEnemyPhase.AddListener(playerTeam.LockUnits);
    }

}
