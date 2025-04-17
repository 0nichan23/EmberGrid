using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBarSlot : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    private Hero refHero;


    public void Setup(Hero givenHero)
    {
        refHero = givenHero;
        image.sprite = refHero.Visual.sprite;
        image.color = refHero.Visual.color;
        button.onClick.AddListener(OnClick);
    }


    public void OnClick()
    {
        GameManager.Instance.SelectionManager.SelectUnit(refHero);
        //select the hero
    }

}
