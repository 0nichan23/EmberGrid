using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private UnitTeam enemyTeam;
    [SerializeField] private EnemyPhaseController phaseController;

    public UnitTeam Team { get => enemyTeam; }
    public EnemyPhaseController PhaseController { get => phaseController; }

    private void Start()
    {
        enemyTeam.Setup();
    }

}
