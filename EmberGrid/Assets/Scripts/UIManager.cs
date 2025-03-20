using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnitPanel unitPanel;

    public UnitPanel UnitPanel { get => unitPanel;}


    public void OpenUnitPanel(Unit givenUnit)
    {
        UnitPanel.Setup(givenUnit);
    }


}
