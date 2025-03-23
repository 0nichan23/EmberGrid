using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridBuilder gridBuilder;

    public SelectionManager SelectionManager { get => selectionManager; }
    public UIManager UIManager { get => uiManager; }
    public GridBuilder GridBuilder { get => gridBuilder; }

    private void Start()
    {
        selectionManager.OnSelectUnit.AddListener(uiManager.OpenUnitPanel);

    }
}
