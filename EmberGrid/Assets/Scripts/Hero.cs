using UnityEngine;
using UnityEngine.Jobs;

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
            ApplyLoadout();
        }
    }

    private void ApplyLoadout()
    {
        if (loadout == null)
            return;

        WeaponHandler.CacheAction(loadout.PrimaryAtWill);
        WeaponHandler.CacheAction(loadout.PrimaryEncounter);
        WeaponHandler.CacheAction(loadout.PrimaryUltimate);
        WeaponHandler.CacheAction(loadout.SecondaryAtWill);
        WeaponHandler.CacheAction(loadout.SecondaryEncounter);
        WeaponHandler.CacheAction(loadout.SecondaryUltimate);

        foreach (var item in loadout.UtilityActions)
        {
            WeaponHandler.CacheAction(item);
        }
        // Apply class passives
        foreach (var passive in loadout.PassiveEffects)
        {
            passive.Apply(this);
        }
    }

    protected override void Events()
    {
        base.Events();
        OnSelected.AddListener(() => GameManager.Instance.UIManager.ActionBar.Setup(this));
    }
}
