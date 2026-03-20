using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroPortraitSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image portrait;
    [SerializeField] private Button button;
    [SerializeField] private GameObject highlight;

    private int rosterIndex;
    private Canvas rootCanvas;

    private GameObject dragIcon;

    public int RosterIndex => rosterIndex;

    public void Setup(int index, HeroProgression hero)
    {
        rosterIndex = index;
        portrait.sprite = hero.Definition.Portrait;
        rootCanvas = GetComponentInParent<Canvas>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        TavernManager.Instance.SelectHero(rosterIndex);
    }

    public void SetHighlight(bool active)
    {
        highlight.SetActive(active);
    }

    // --- Drag support (for dragging into PartySlots) ---

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(rootCanvas.transform, false);

        var img = dragIcon.AddComponent<Image>();
        img.sprite = portrait.sprite;
        img.raycastTarget = false;
        img.SetNativeSize();

        var rect = dragIcon.GetComponent<RectTransform>();
        rect.sizeDelta = portrait.rectTransform.sizeDelta;
        rect.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            Destroy(dragIcon);
    }
}
