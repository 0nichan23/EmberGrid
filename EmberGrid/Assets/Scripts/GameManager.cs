using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private UIManager uiManager;

    public SelectionManager SelectionManager { get => selectionManager; }
    public UIManager UIManager { get => uiManager; }


    private void Start()
    {
        selectionManager.OnSelectUnit.AddListener(uiManager.OpenUnitPanel);

    }
}
