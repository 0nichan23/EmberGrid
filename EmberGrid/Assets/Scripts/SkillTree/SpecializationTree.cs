using UnityEngine;

[CreateAssetMenu(fileName = "SpecTree", menuName = "SkillTree/SpecializationTree")]
public class SpecializationTree : ScriptableObject
{
    [SerializeField] private string specName;
    [SerializeField] private string specDescription;
    [SerializeField] private Sprite specIcon;
    [SerializeField] private SkillTreeLine[] specLines;

    public string SpecName { get => specName; }
    public string SpecDescription { get => specDescription; }
    public Sprite SpecIcon { get => specIcon; }
    public SkillTreeLine[] SpecLines { get => specLines; }
}
