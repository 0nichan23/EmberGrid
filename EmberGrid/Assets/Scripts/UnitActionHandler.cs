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
        actionPoints = 1;
    }

}
