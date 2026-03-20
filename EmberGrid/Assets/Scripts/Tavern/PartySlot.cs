using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private int slotIndex;

    public void Refresh(HeroProgression hero)
    {
        portrait.sprite = hero.Definition.Portrait;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag;
        if (dragged == null) return;

        var portraitSlot = dragged.GetComponent<HeroPortraitSlot>();
        if (portraitSlot == null) return;

        TavernManager.Instance.SwapPartyMember(slotIndex, portraitSlot.RosterIndex);
    }
}
