using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{

    private Unit selectedUnit;

    public Unit SelectedUnit { get => selectedUnit; }


    public void SelectUnit(Unit givenUnit)
    {
        if (!ReferenceEquals(selectedUnit, null))
        {
            selectedUnit.OnDeselected?.Invoke();
        }

        selectedUnit = givenUnit;

        if (!ReferenceEquals(selectedUnit, null))
        {
            selectedUnit.OnSelected?.Invoke();
        }
    }

}
