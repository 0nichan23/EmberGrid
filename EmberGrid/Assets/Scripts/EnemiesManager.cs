using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private UnitTeam EnemyTeam;

    public UnitTeam Team { get => EnemyTeam; }

    private void Start()
    {
        EnemyTeam.Setup();
    }

}
