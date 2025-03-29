using UnityEngine;


[CreateAssetMenu(fileName = "UnitAction", menuName = "Action")]
public class UnitAction : ScriptableObject
{
    [SerializeField] private int cost;
    [SerializeField] private ActionType actionType;
    [SerializeField] private ActionEffect[] effects;
    [SerializeField] private Vector2Int[] targets;
    [SerializeField] private bool targeted;
    [SerializeField] private int range;
    public int Cost { get => cost; }
    public ActionType ActionType { get => actionType; }
    public ActionEffect[] Effects { get => effects; }
    public Vector2Int[] Targets { get => targets; }
    public bool Targeted { get => targeted; }
    public int Range { get => range; }
}
public enum ActionType
{
    Power,
    Spell
}

