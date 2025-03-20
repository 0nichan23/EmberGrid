using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] private Image unitPortrait;

    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitClass;

    [SerializeField] private TextMeshProUGUI mightStat;
    [SerializeField] private TextMeshProUGUI finesseStat;
    [SerializeField] private TextMeshProUGUI magicStat;
    [SerializeField] private TextMeshProUGUI resilianceStat;
    [SerializeField] private TextMeshProUGUI resistanceStat;
    [SerializeField] private TextMeshProUGUI willpowerStat;

    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image damageTypeIcon;
    [SerializeField] private Image physicalDefenseIcon;
    [SerializeField] private Image magicDefenseIcon;

    [SerializeField] private TextMeshProUGUI basicAttackDamage;
    [SerializeField] private TextMeshProUGUI critChance;
    [SerializeField] private TextMeshProUGUI physicalDefence;
    [SerializeField] private TextMeshProUGUI magicalDefence;


    public void Setup(Unit givenUnit)
    {
        unitName.text = givenUnit.name;
        mightStat.text = givenUnit.Stats.Might.ToString();
        finesseStat.text = givenUnit.Stats.Finesse.ToString();
        magicStat.text = givenUnit.Stats.Magic.ToString();
        resilianceStat.text = givenUnit.Stats.Resilience.ToString();
        resistanceStat.text = givenUnit.Stats.Resistance.ToString();
        willpowerStat.text = givenUnit.Stats.Willpower.ToString();
        critChance.text = givenUnit.Stats.CritHit.ToString("F1");
        physicalDefence.text = givenUnit.Stats.PhysicalDefence.ToString("F1");
        magicalDefence.text = givenUnit.Stats.MagicDefence.ToString("F1");

        gameObject.SetActive(true);
    }


}
