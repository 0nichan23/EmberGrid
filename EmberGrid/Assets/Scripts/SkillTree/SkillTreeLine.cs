using UnityEngine;

[CreateAssetMenu(fileName = "SkillTreeLine", menuName = "SkillTree/Line")]
public class SkillTreeLine : ScriptableObject
{
    [SerializeField] private string lineName;
    [SerializeField] private int requiredLevel;
    [SerializeField] private SkillTreeNode[] choices;

    public string LineName { get => lineName; }
    public int RequiredLevel { get => requiredLevel; }
    public SkillTreeNode[] Choices { get => choices; }
}
