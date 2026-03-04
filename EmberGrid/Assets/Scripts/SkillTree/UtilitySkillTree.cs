using UnityEngine;

[CreateAssetMenu(fileName = "UtilitySkillTree", menuName = "SkillTree/UtilitySkillTree")]
public class UtilitySkillTree : ScriptableObject
{
    [SerializeField] private SkillTreeLine line1;
    [SerializeField] private SkillTreeLine line2;

    public SkillTreeLine Line1 { get => line1; }
    public SkillTreeLine Line2 { get => line2; }
}
