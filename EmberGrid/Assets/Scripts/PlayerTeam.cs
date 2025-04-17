using UnityEngine;

[System.Serializable]
public class PlayerTeam
{
    [SerializeField] private Hero[] units;

    public Hero[] Units { get => units; }

    public void Setup()
    {
        foreach (var unit in units)
        {
            unit.OnTurnEnded.AddListener(CheckPhaseEnded);
        }
        PositionUnits();
        UnLockUnits();
    }


    private void PositionUnits()
    {
        foreach (var unit in units)
        {
            unit.Movement.SetStartPos(GameManager.Instance.GridBuilder.GetRandomTile());
        }
    }

    private void CheckPhaseEnded()
    {
        foreach(var unit in units)
        {
            if (!unit.ActionHandler.TurnPlayed)
            {
                return;
            }
        }
        GameManager.Instance.TurnManager.SwitchPhase();
    }


    public void LockUnits()
    {
        foreach (var unit in units)
        {
            unit.Selector.Selectable = false;
        }
    }

    public void UnLockUnits()
    {
        foreach (var unit in units)
        {
            unit.Selector.Selectable = true;
        }
    }

    public void PlayerPhaseReset()
    {
        foreach (var item in units)
        {
            item.ActionHandler.BeginPhase();
            item.Movement.ResetSpeed();
        }
    }

}
