using UnityEngine;

public abstract class SkillTreeNode : ScriptableObject
{
    [SerializeField] private string nodeName;
    [SerializeField] [TextArea] private string description;
    [SerializeField] private Sprite icon;

    public string NodeName { get => nodeName; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
}
