[System.Serializable]
public class ActiveStatusEffect
{
    public StatusEffects Type;
    public int Stacks;

    public ActiveStatusEffect(StatusEffects type, int stacks)
    {
        Type = type;
        Stacks = stacks;
    }

    public ActiveStatusEffect Copy()
    {
        return new ActiveStatusEffect(Type, Stacks);
    }
}
