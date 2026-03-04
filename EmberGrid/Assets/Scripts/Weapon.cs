using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;

    [Header("Skill Tree (Heroes)")]
    [SerializeField] private WeaponSkillTree skillTree;
    [SerializeField] private UtilitySkillTree utilityTree;

    [Header("Direct Abilities (Enemies)")]
    [SerializeField] private UnitAction basicAttack;
    [SerializeField] private UnitAction[] encounters;
    [SerializeField] private UnitAction ultimate;

    public string WeaponName { get => weaponName; }
    public Sprite Icon { get => icon; }
    public WeaponSkillTree SkillTree { get => skillTree; }
    public UtilitySkillTree UtilityTree { get => utilityTree; }
    public bool IsUtility => utilityTree != null;
    public bool HasSkillTree => skillTree != null;

    public UnitAction BasicAttack { get => basicAttack; }
    public UnitAction[] Encounters { get => encounters; }
    public UnitAction Daily { get => ultimate; }
}
