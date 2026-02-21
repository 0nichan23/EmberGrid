using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RewindManager : MonoBehaviour
{
    [SerializeField] private int maxRewindsPerPhase = 3;
    [SerializeField] private Button rewindButton;
    [SerializeField] private TextMeshProUGUI rewindCountText;

    private Stack<PhaseSnapshot> snapshotStack = new Stack<PhaseSnapshot>();
    private int rewindsUsedThisPhase;
    private bool isRewinding;

    public int RewindsRemaining => maxRewindsPerPhase - rewindsUsedThisPhase;
    public bool CanRewind => snapshotStack.Count > 0 && rewindsUsedThisPhase < maxRewindsPerPhase;

    private void Start()
    {
        GameManager.Instance.TurnManager.OnPlayerPhase.AddListener(OnPlayerPhaseStart);

        if (rewindButton != null)
        {
            rewindButton.onClick.AddListener(Rewind);
        }

        UpdateRewindUI();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb != null && kb.zKey.wasPressedThisFrame &&
            (kb.leftCtrlKey.isPressed || kb.rightCtrlKey.isPressed))
        {
            Rewind();
        }
    }

    private void OnPlayerPhaseStart()
    {
        if (!isRewinding)
        {
            snapshotStack.Clear();
            rewindsUsedThisPhase = 0;
        }
        UpdateRewindUI();
    }

    public void CaptureSnapshot()
    {
        if (GameManager.Instance.TurnManager.CurrentPhase != Phase.Player)
            return;

        var snapshot = new PhaseSnapshot();

        Unit[] heroes = GameManager.Instance.PlayerManager.Team.Units;
        snapshot.HeroSnapshots = new UnitSnapshot[heroes.Length];
        for (int i = 0; i < heroes.Length; i++)
        {
            snapshot.HeroSnapshots[i] = UnitSnapshot.Capture(heroes[i]);
        }

        Unit[] enemies = GameManager.Instance.EnemiesManager.Team.Units;
        snapshot.EnemySnapshots = new UnitSnapshot[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            snapshot.EnemySnapshots[i] = UnitSnapshot.Capture(enemies[i]);
        }

        snapshot.SelectedUnit = GameManager.Instance.SelectionManager.SelectedUnit;

        snapshotStack.Push(snapshot);
        UpdateRewindUI();
    }

    public void Rewind()
    {
        if (!CanRewind) return;

        PhaseSnapshot snapshot = snapshotStack.Pop();
        rewindsUsedThisPhase++;

        GameManager.Instance.SelectionManager.SelectUnit(null);

        bool wasEnemyPhase = GameManager.Instance.TurnManager.CurrentPhase == Phase.Enemy;

        if (wasEnemyPhase)
        {
            GameManager.Instance.EnemiesManager.PhaseController.CancelPhase();

            isRewinding = true;
            GameManager.Instance.TurnManager.SwitchPhase();
            isRewinding = false;
        }

        foreach (var us in snapshot.HeroSnapshots)
        {
            us.Restore();
        }

        foreach (var us in snapshot.EnemySnapshots)
        {
            us.Restore();
        }

        if (snapshot.SelectedUnit != null)
        {
            GameManager.Instance.SelectionManager.SelectUnit(snapshot.SelectedUnit);
        }

        UpdateRewindUI();
    }

    private void UpdateRewindUI()
    {
        if (rewindButton != null)
        {
            rewindButton.interactable = CanRewind;
        }

        if (rewindCountText != null)
        {
            rewindCountText.text = $"{RewindsRemaining}/{maxRewindsPerPhase}";
        }
    }
}
