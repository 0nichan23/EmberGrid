using UnityEngine;
using UnityEngine.UI;

public class TavernCharacterPanel : MonoBehaviour
{
    [Header("Hero Portraits")]
    [SerializeField] private Transform portraitRowParent;
    [SerializeField] private HeroPortraitSlot portraitSlotPrefab;

    [Header("Party Slots")]
    [SerializeField] private PartySlot[] partySlots;

    [Header("Tabs")]
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private GameObject[] tabHighlights;
    [SerializeField] private GameObject[] tabContents;

    private HeroPortraitSlot[] portraitSlots;
    private int activeTabIndex;

    private void Start()
    {
        SetupPortraitRow();
        SetupPartySlots();
        SetupTabs();

        TavernManager.Instance.OnHeroSelected += OnHeroSelected;
        TavernManager.Instance.OnPartyChanged += RefreshPartySlots;

        // Highlight the initially selected hero
        OnHeroSelected(TavernManager.Instance.SelectedHeroIndex);
        SetActiveTab(0);
    }

    private void OnDestroy()
    {
        if (TavernManager.Instance != null)
        {
            TavernManager.Instance.OnHeroSelected -= OnHeroSelected;
            TavernManager.Instance.OnPartyChanged -= RefreshPartySlots;
        }
    }

    // --- Portrait Row ---

    private void SetupPortraitRow()
    {
        var heroes = TavernManager.Instance.AllHeroes;
        portraitSlots = new HeroPortraitSlot[heroes.Length];

        for (int i = 0; i < heroes.Length; i++)
        {
            portraitSlots[i] = Instantiate(portraitSlotPrefab, portraitRowParent);
            portraitSlots[i].Setup(i, heroes[i]);
        }
    }

    private void OnHeroSelected(int rosterIndex)
    {
        for (int i = 0; i < portraitSlots.Length; i++)
        {
            portraitSlots[i].SetHighlight(i == rosterIndex);
        }

        RefreshActiveTab();
    }

    // --- Party Slots ---

    private void SetupPartySlots()
    {
        RefreshPartySlots();
    }

    private void RefreshPartySlots()
    {
        for (int i = 0; i < partySlots.Length; i++)
        {
            var hero = TavernManager.Instance.GetPartyHero(i);
            partySlots[i].Refresh(hero);
        }
    }

    // --- Tabs ---

    private void SetupTabs()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i;
            tabButtons[i].onClick.AddListener(() => SetActiveTab(index));
        }
    }

    private void SetActiveTab(int index)
    {
        activeTabIndex = index;

        for (int i = 0; i < tabContents.Length; i++)
        {
            tabContents[i].SetActive(i == index);
            tabHighlights[i].SetActive(i == index);
        }

        RefreshActiveTab();
    }

    private void RefreshActiveTab()
    {
        // Future: notify the active tab content to refresh with the selected hero's data.
        // Each tab (Equipment, ClassTree, Vanity) will implement its own refresh logic.
    }
}
