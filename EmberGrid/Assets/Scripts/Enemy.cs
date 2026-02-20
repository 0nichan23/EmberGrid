using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    private EnemyAI enemyAI;

    public EnemyAI EnemyAI { get => enemyAI;  }

    protected override void Start()
    {
        base.Start();
        enemyAI = new EnemyAI(this);
    }

    protected override void Events()
    {
        OnTurnEnded.AddListener(() => CurrentMode = ActiveMode.Unselected);
        OnDeselected.AddListener(() => CurrentMode = ActiveMode.Unselected);
    }

}
