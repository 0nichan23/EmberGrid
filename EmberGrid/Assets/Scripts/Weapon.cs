using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private UnitAction basicAttack;
    [SerializeField] private UnitAction encounters;
    [SerializeField] private UnitAction ultimate;

    public UnitAction BasicAttack { get => basicAttack; }
    public UnitAction Encounters { get => encounters; }
    public UnitAction Daily { get => ultimate; }
}
