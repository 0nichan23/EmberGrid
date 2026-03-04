using UnityEngine;

[CreateAssetMenu(fileName = "AbilityNode", menuName = "SkillTree/Nodes/Ability")]
public class AbilityNode : SkillTreeNode
{
    [SerializeField] private UnitAction grantedAction;

    public UnitAction GrantedAction { get => grantedAction; }
}
