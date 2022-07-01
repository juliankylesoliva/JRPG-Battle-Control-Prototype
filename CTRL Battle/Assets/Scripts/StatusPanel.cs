using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    private BattleUnit givenUnit = null;

    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text mpText;

    [SerializeField] Transform hpFill;
    [SerializeField] Transform mpFill;

    void Update()
    {
        if (givenUnit != null)
        {
            UpdatePanel();
        }
        else
        {
            ShowBlankPanel();
        }
    }

    private void UpdatePanel()
    {
        nameText.text = givenUnit.CharacterName;
        hpText.text = $"{givenUnit.Health}/{givenUnit.MaxHealth}";
        mpText.text = $"{givenUnit.Magic}/{givenUnit.MaxMagic}";
        hpFill.localScale = new Vector3(GetHPRatio(givenUnit), 1f, 1f);
        mpFill.localScale = new Vector3(GetMPRatio(givenUnit), 1f, 1f);
    }

    private void ShowBlankPanel()
    {
        nameText.text = "???";
        hpText.text = "--/--";
        mpText.text = "--/--";
        hpFill.localScale = Vector3.zero;
        mpFill.localScale = Vector3.zero;
    }

    private float GetHPRatio(BattleUnit unit)
    {
        return (((float)unit.Health)/((float)unit.MaxHealth));
    }

    private float GetMPRatio(BattleUnit unit)
    {
        return (((float)unit.Magic) / ((float)unit.MaxMagic));
    }

    public void SetBattleUnit(BattleUnit unit)
    {
        givenUnit = unit;
    }
}
