using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnitPanel unitPanel;
    [SerializeField] private ActionBar actionBar;

    public UnitPanel UnitPanel { get => unitPanel;}
    public ActionBar ActionBar { get => actionBar; }

    public void OpenUnitPanel(Unit givenUnit)
    {
        UnitPanel.gameObject.SetActive(true);
        UnitPanel.Setup(givenUnit);
    }

    public void CloseUnitPanel(Unit givenUnit)
    {
        UnitPanel.gameObject.SetActive(false);
    }



}
