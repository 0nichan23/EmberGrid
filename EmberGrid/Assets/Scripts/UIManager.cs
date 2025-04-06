using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnitPanel unitPanel;
    [SerializeField] private GameObject ActionBar;

    public UnitPanel UnitPanel { get => unitPanel;}
    public GameObject ActionBar1 { get => ActionBar; }

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
