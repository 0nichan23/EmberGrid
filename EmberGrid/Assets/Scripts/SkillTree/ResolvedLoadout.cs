using System.Collections.Generic;

public class ResolvedLoadout
{
    private UnitAction primaryAtWill;
    private UnitAction primaryEncounter;
    private UnitAction primaryUltimate;

    private UnitAction secondaryAtWill;
    private UnitAction secondaryEncounter;
    private UnitAction secondaryUltimate;

    private List<UnitAction> utilityActions;
    private List<PassiveEffect> passiveEffects;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;
    private Weapon utility;

    public UnitAction PrimaryAtWill { get => primaryAtWill; }
    public UnitAction PrimaryEncounter { get => primaryEncounter; }
    public UnitAction PrimaryUltimate { get => primaryUltimate; }
    public UnitAction SecondaryAtWill { get => secondaryAtWill; }
    public UnitAction SecondaryEncounter { get => secondaryEncounter; }
    public UnitAction SecondaryUltimate { get => secondaryUltimate; }
    public List<UnitAction> UtilityActions { get => utilityActions; }
    public List<PassiveEffect> PassiveEffects { get => passiveEffects; }
    public Weapon PrimaryWeapon { get => primaryWeapon; }
    public Weapon SecondaryWeapon { get => secondaryWeapon; }
    public Weapon Utility { get => utility; }

    public static ResolvedLoadout Resolve(HeroProgression progression)
    {
        var loadout = new ResolvedLoadout();
        int level = progression.Level;

        // Resolve primary weapon
        loadout.primaryWeapon = progression.SelectedPrimary;
        if (loadout.primaryWeapon.SkillTree != null)
        {
            var tree = loadout.primaryWeapon.SkillTree;
            loadout.primaryAtWill = ResolveAbilityFromLine(
                tree.AtWillLine, progression.PrimaryWeaponSelections, 0, level);
            loadout.primaryEncounter = ResolveAbilityFromLine(
                tree.EncounterLine, progression.PrimaryWeaponSelections, 1, level);
            loadout.primaryUltimate = ResolveAbilityFromLine(
                tree.UltimateLine, progression.PrimaryWeaponSelections, 2, level);
        }

        // Resolve secondary weapon
        loadout.secondaryWeapon = progression.SelectedSecondary;
        if (loadout.secondaryWeapon.SkillTree != null)
        {
            var tree = loadout.secondaryWeapon.SkillTree;
            loadout.secondaryAtWill = ResolveAbilityFromLine(
                tree.AtWillLine, progression.SecondaryWeaponSelections, 0, level);
            loadout.secondaryEncounter = ResolveAbilityFromLine(
                tree.EncounterLine, progression.SecondaryWeaponSelections, 1, level);
            loadout.secondaryUltimate = ResolveAbilityFromLine(
                tree.UltimateLine, progression.SecondaryWeaponSelections, 2, level);
        }

        // Resolve utility
        loadout.utility = progression.Utility;
        loadout.utilityActions = new List<UnitAction>();
        if (loadout.utility != null && loadout.utility.UtilityTree != null)
        {
            var u1 = ResolveAbilityFromLine(
                loadout.utility.UtilityTree.Line1, progression.UtilitySelections, 0, level);
            var u2 = ResolveAbilityFromLine(
                loadout.utility.UtilityTree.Line2, progression.UtilitySelections, 1, level);
            if (u1 != null) loadout.utilityActions.Add(u1);
            if (u2 != null) loadout.utilityActions.Add(u2);
        }

        // Resolve class passives
        loadout.passiveEffects = new List<PassiveEffect>();
        var classTree = progression.Definition.ClassTree;
        if (classTree != null)
        {
            // Base tree
            for (int i = 0; i < classTree.BaseLines.Length; i++)
            {
                var line = classTree.BaseLines[i];
                if (level < line.RequiredLevel) break;

                int choiceIdx = progression.ClassBaseSelections.GetSelection(i);
                if (choiceIdx >= 0 && choiceIdx < line.Choices.Length)
                {
                    if (line.Choices[choiceIdx] is PassiveNode passiveNode)
                    {
                        loadout.passiveEffects.AddRange(passiveNode.Effects);
                    }
                }
            }

            // Spec tree
            var spec = progression.SelectedSpec;
            if (spec != null)
            {
                for (int i = 0; i < spec.SpecLines.Length; i++)
                {
                    var line = spec.SpecLines[i];
                    if (level < line.RequiredLevel) break;

                    int choiceIdx = progression.ClassSpecSelections.GetSelection(i);
                    if (choiceIdx >= 0 && choiceIdx < line.Choices.Length)
                    {
                        if (line.Choices[choiceIdx] is PassiveNode passiveNode)
                        {
                            loadout.passiveEffects.AddRange(passiveNode.Effects);
                        }
                    }
                }
            }
        }

        return loadout;
    }

    private static UnitAction ResolveAbilityFromLine(
        SkillTreeLine line, TreeSelections selections, int lineIndex, int heroLevel)
    {
        if (line == null || heroLevel < line.RequiredLevel)
            return null;

        int choiceIdx = selections.GetSelection(lineIndex);
        if (choiceIdx < 0 || choiceIdx >= line.Choices.Length)
            return null;

        if (line.Choices[choiceIdx] is AbilityNode abilityNode)
            return abilityNode.GrantedAction;

        return null;
    }
}
