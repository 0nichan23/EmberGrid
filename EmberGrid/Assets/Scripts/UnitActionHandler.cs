using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitActionHandler
{
    public UnityEvent<Unit> OnWaited;

    [SerializeField] private int actionPoints;
    private Unit owner;
    private bool turnPlayed;
    public bool CanTakeAction => actionPoints > 0;
    public int ActionPoints { get => actionPoints; set => actionPoints = value; }

    public bool TurnPlayed { get => turnPlayed; set => turnPlayed = value; }


    public UnitActionHandler(Unit owner)
    {
        this.owner = owner;
        actionPoints = 1;
    }

    public void ExpandAction()
    {
        actionPoints -= 1;
        if (actionPoints == 0)
        {
            EndTurn();
        }
    }

    public void TakeWaitAction()
    {
        if (CanTakeAction)
        {
            GameManager.Instance.RewindManager.CaptureSnapshot();
            owner.Movement.CancelMovementMode();
            owner.WeaponHandler.CancelAttackMode();
            ExpandAction();
            OnWaited?.Invoke(owner);
        }
    }

    private void EndTurn()
    {
        TurnPlayed = true;
        owner.OnTurnEnded?.Invoke();
    }

    public void BeginPhase()
    {
        // Stun: unit cannot act this phase
        if (owner.HasStatus(StatusEffects.Stun))
        {
            actionPoints = 0;
            TurnPlayed = true;
            Debug.Log($"{owner.name} is stunned and cannot act");
            return;
        }

        actionPoints = 1;
        TurnPlayed = false;
        // Haste AP bonus is applied by StatusEffectProcessor.ProcessPhaseStart
    }

    /// <summary>
    /// Called when Stun is cleansed mid-phase, re-enabling the unit.
    /// </summary>
    public void OnStunCleansed()
    {
        actionPoints = 1;
        TurnPlayed = false;
        Debug.Log($"{owner.name} was cleansed of stun and can now act");
    }

}
