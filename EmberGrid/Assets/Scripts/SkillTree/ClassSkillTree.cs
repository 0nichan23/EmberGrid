using UnityEngine;

[CreateAssetMenu(fileName = "ClassSkillTree", menuName = "SkillTree/ClassSkillTree")]
public class ClassSkillTree : ScriptableObject
{
    [SerializeField] private SkillTreeLine[] baseLines;
    [SerializeField] private SpecializationTree[] specializations;

    public SkillTreeLine[] BaseLines { get => baseLines; }
    public SpecializationTree[] Specializations { get => specializations; }
}
