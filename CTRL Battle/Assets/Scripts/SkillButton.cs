using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    [SerializeField] TMP_Text skillNameText;
    [SerializeField] TMP_Text skillCostText;

    public void SetSkillNameText(string name)
    {
        skillNameText.text = name;
    }

    public void SetMPCostText(int cost)
    {
        skillCostText.text = $"{cost} MP";
    }

    public void SetHPCostText(int cost)
    {
        skillCostText.text = $"{cost} HP";
    }

    public void SetTextColor(Color color)
    {
        skillNameText.color = color;
        skillCostText.color = color;
    }
}
