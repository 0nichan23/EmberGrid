using UnityEngine;

[CreateAssetMenu(fileName = "PassiveNode", menuName = "SkillTree/Nodes/Passive")]
public class PassiveNode : SkillTreeNode
{
    [SerializeField] private PassiveEffect[] effects;

    public PassiveEffect[] Effects { get => effects; }
}
