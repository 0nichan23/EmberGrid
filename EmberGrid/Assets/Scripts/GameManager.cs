using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private SelectionManager selectionManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridBuilder gridBuilder;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private InputManager inputManager;
    private Utilities utils = new Utilities();

    public SelectionManager SelectionManager { get => selectionManager; }
    public UIManager UIManager { get => uiManager; }
    public GridBuilder GridBuilder { get => gridBuilder; }
    public PlayerManager PlayerManager { get => playerManager; }
    public TurnManager TurnManager { get => turnManager; }
    public InputManager InputManager { get => inputManager; }
}
