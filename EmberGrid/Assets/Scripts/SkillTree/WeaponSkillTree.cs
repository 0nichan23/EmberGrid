using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSkillTree", menuName = "SkillTree/WeaponSkillTree")]
public class WeaponSkillTree : ScriptableObject
{
    [SerializeField] private SkillTreeLine atWillLine;
    [SerializeField] private SkillTreeLine encounterLine;
    [SerializeField] private SkillTreeLine ultimateLine;

    public SkillTreeLine AtWillLine { get => atWillLine; }
    public SkillTreeLine EncounterLine { get => encounterLine; }
    public SkillTreeLine UltimateLine { get => ultimateLine; }
}
