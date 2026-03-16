using UnityEngine;

public class Hero : Unit
{
    [SerializeField] private HeroDefinition heroDefinition;
    private HeroProgression progression;
    private ResolvedLoadout loadout;

    public HeroDefinition HeroDefinition { get => heroDefinition; }
    public HeroProgression Progression { get => progression; }

    public void SetProgression(HeroProgression progression)
    {
        this.progression = progression;
    }

    protected override void Start()
    {
        base.Start();

        if (progression != null)
        {
            loadout = ResolvedLoadout.Resolve(progression);
        }
    }

    protected override void Events()
    {
        base.Events();
        OnSelected.AddListener(() => GameManager.Instance.UIManager.ActionBar.Setup(this));
    }
}
