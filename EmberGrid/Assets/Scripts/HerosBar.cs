using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerosBar : MonoBehaviour
{
    [SerializeField] private HeroBarSlot[] slots;
    [SerializeField] private Transform slotsParent;
    [SerializeField] private HeroBarSlot herBarSlotPrefab;
    public void CreateSlots(Hero[] team)
    {
        //will always have 3 slots and 3 heros at the start. 
        slots = new HeroBarSlot[team.Length];
        for (int i = 0; i < slots.Length; i++) 
        {
            slots[i] = Instantiate(herBarSlotPrefab, slotsParent);
            slots[i].Setup(team[i]);
        }
    }

}
