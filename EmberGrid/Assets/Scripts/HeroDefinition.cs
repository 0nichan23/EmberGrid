using UnityEngine;

[CreateAssetMenu(fileName = "HeroDefinition", menuName = "HeroDefinition")]
public class HeroDefinition : ScriptableObject
{
    [SerializeField] private string className;
    [SerializeField] private BaseStats baseStats;
    [SerializeField] private Weapon[] primaryWeapons;
    [SerializeField] private Weapon[] secondaryWeapons;
    [SerializeField] private Weapon utilityItem;
    [SerializeField] private ClassSkillTree classTree;
    [SerializeField] private Sprite portrait;

    public string ClassName { get => className; }
    public BaseStats BaseStats { get => baseStats; }
    public Weapon[] PrimaryWeapons { get => primaryWeapons; }
    public Weapon[] SecondaryWeapons { get => secondaryWeapons; }
    public Weapon UtilityItem { get => utilityItem; }
    public ClassSkillTree ClassTree { get => classTree; }
    public Sprite Portrait { get => portrait; }
}
