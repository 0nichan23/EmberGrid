using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UnitPanel unitPanel;
    [SerializeField] private ActionBar actionBar;
    [SerializeField] private HerosBar herosBar;

    public UnitPanel UnitPanel { get => unitPanel; }
    public ActionBar ActionBar { get => actionBar; }
    public HerosBar HerosBar { get => herosBar; }

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
