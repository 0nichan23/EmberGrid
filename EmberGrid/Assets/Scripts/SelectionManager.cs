using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public UnityEvent<Unit> OnSelectUnit;
    public UnityEvent<Unit> OnDeselectedUnit;

    private Unit selectedUnit;

    public Unit SelectedUnit { get => selectedUnit; }


    public void SelectUnit(Unit givenUnit)
    {
        if (!ReferenceEquals(selectedUnit, null))
        {
            OnDeselectedUnit?.Invoke(selectedUnit);
        }

        selectedUnit = givenUnit;

        if (!ReferenceEquals(selectedUnit, null))
        {
            OnSelectUnit?.Invoke(selectedUnit);
            selectedUnit.OnSelected?.Invoke();
        }
    }

}
