using System;
using UnityEngine;

public class TavernManager : MonoBehaviour
{
    [SerializeField] private HeroProgression[] allHeroes;
    [SerializeField] private int[] partyIndices = new int[3];

    private int selectedHeroIndex;

    public event Action<int> OnHeroSelected;
    public event Action OnPartyChanged;

    public HeroProgression[] AllHeroes => allHeroes;
    public int[] PartyIndices => partyIndices;
    public int SelectedHeroIndex => selectedHeroIndex;

    public static TavernManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        SelectHero(0);
    }

    public HeroProgression GetSelectedProgression()
    {
        return allHeroes[selectedHeroIndex];
    }

    public void SelectHero(int rosterIndex)
    {
        selectedHeroIndex = rosterIndex;
        OnHeroSelected?.Invoke(rosterIndex);
    }

    public HeroProgression GetPartyHero(int slotIndex)
    {
        return allHeroes[partyIndices[slotIndex]];
    }

    public bool IsInParty(int rosterIndex)
    {
        for (int i = 0; i < partyIndices.Length; i++)
        {
            if (partyIndices[i] == rosterIndex)
                return true;
        }
        return false;
    }

    public int GetPartySlot(int rosterIndex)
    {
        for (int i = 0; i < partyIndices.Length; i++)
        {
            if (partyIndices[i] == rosterIndex)
                return i;
        }
        return -1;
    }

    public void SwapPartyMember(int slotIndex, int rosterIndex)
    {
        int existingSlot = GetPartySlot(rosterIndex);

        if (existingSlot >= 0)
        {
            // Hero is already in party — swap the two slots
            int temp = partyIndices[slotIndex];
            partyIndices[slotIndex] = partyIndices[existingSlot];
            partyIndices[existingSlot] = temp;
        }
        else
        {
            partyIndices[slotIndex] = rosterIndex;
        }

        OnPartyChanged?.Invoke();
    }
}
