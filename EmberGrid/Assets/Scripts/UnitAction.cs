using UnityEngine;


[CreateAssetMenu(fileName = "UnitAction", menuName = "Action")]
public class UnitAction : ScriptableObject
{
    [SerializeField] private int cost;
    [SerializeField] private ActionType actionType;
    [SerializeField] private ActionEffect[] effects;

    //powers cost durability, spells cost mana

    public int Cost { get => cost; }
    public ActionType ActionType { get => actionType; }
    public ActionEffect[] Effects { get => effects; }
}
public enum ActionType
{
    Power,
    Spell
}
