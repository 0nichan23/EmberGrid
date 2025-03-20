using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public UnityEvent<Unit> OnSelectUnit;

    private Unit selectedUnit;

    public Unit SelectedUnit { get => selectedUnit; }


    public void SelectUnit(Unit givenUnit)
    {
        selectedUnit = givenUnit;

        OnSelectUnit?.Invoke(selectedUnit);
    }

}
