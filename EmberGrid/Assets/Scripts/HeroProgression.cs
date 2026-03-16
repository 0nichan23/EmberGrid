using UnityEngine;

[System.Serializable]
public class HeroProgression
{
    [SerializeField] private HeroDefinition definition;
    [SerializeField] private int level;
    [SerializeField] private int xp;

    [SerializeField] private int selectedPrimaryIndex;
    [SerializeField] private int selectedSecondaryIndex;

    [SerializeField] private TreeSelections primaryWeaponSelections;
    [SerializeField] private TreeSelections secondaryWeaponSelections;
    [SerializeField] private TreeSelections utilitySelections;
    [SerializeField] private TreeSelections classBaseSelections;
    [SerializeField] private int selectedSpecIndex;
    [SerializeField] private TreeSelections classSpecSelections;

    public HeroDefinition Definition { get => definition; }
    public int Level { get => level; set => level = value; }
    public int XP { get => xp; set => xp = value; }
    public int SelectedPrimaryIndex { get => selectedPrimaryIndex; set => selectedPrimaryIndex = value; }
    public int SelectedSecondaryIndex { get => selectedSecondaryIndex; set => selectedSecondaryIndex = value; }
    public TreeSelections PrimaryWeaponSelections { get => primaryWeaponSelections; }
    public TreeSelections SecondaryWeaponSelections { get => secondaryWeaponSelections; }
    public TreeSelections UtilitySelections { get => utilitySelections; }
    public TreeSelections ClassBaseSelections { get => classBaseSelections; }
    public int SelectedSpecIndex { get => selectedSpecIndex; set => selectedSpecIndex = value; }
    public TreeSelections ClassSpecSelections { get => classSpecSelections; }

    public Weapon SelectedPrimary =>
        definition.PrimaryWeapons != null && selectedPrimaryIndex >= 0 && selectedPrimaryIndex < definition.PrimaryWeapons.Length
            ? definition.PrimaryWeapons[selectedPrimaryIndex] : null;
    public Weapon SelectedSecondary =>
        definition.SecondaryWeapons != null && selectedSecondaryIndex >= 0 && selectedSecondaryIndex < definition.SecondaryWeapons.Length
            ? definition.SecondaryWeapons[selectedSecondaryIndex] : null;
    public Weapon Utility => definition.UtilityItem;
    public SpecializationTree SelectedSpec =>
        selectedSpecIndex >= 0 && definition.ClassTree != null
        && definition.ClassTree.Specializations != null
        && selectedSpecIndex < definition.ClassTree.Specializations.Length
            ? definition.ClassTree.Specializations[selectedSpecIndex] : null;

    public HeroProgression(HeroDefinition definition)
    {
        this.definition = definition;
        level = 1;
        xp = 0;
        selectedPrimaryIndex = 0;
        selectedSecondaryIndex = 0;
        selectedSpecIndex = -1;
        primaryWeaponSelections = new TreeSelections();
        secondaryWeaponSelections = new TreeSelections();
        utilitySelections = new TreeSelections();
        classBaseSelections = new TreeSelections();
        classSpecSelections = new TreeSelections();
    }
}
