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
            ApplyLoadout();
        }
    }

    private void ApplyLoadout()
    {
        if (loadout == null) return;

        // Replace the default WeaponHandler with one using resolved abilities
        if (loadout.PrimaryWeapon != null)
        {
            var wh = new WeaponHandler(this, loadout.PrimaryWeapon,
                loadout.PrimaryAtWill, loadout.PrimaryEncounter, loadout.PrimaryUltimate);
            SetWeaponHandler(wh);
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
