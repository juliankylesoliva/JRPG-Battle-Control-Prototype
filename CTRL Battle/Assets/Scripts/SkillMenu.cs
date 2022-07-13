using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillMenu : MonoBehaviour
{
    [SerializeField] GameObject skillSelectButtonPrefab; // Make a button handler script

    [SerializeField] GameObject skillList;
    [SerializeField] Transform skillButtonGroup;
    [SerializeField] GameObject skillDescriptionBox;
    [SerializeField] TMP_Text skillDescriptionBoxText;

    private BattleMenu battleMenu;
    private BattleSystem battleSystem;

    void Awake()
    {
        battleMenu = this.gameObject.GetComponent<BattleMenu>();
        battleSystem = this.gameObject.GetComponent<BattleSystem>();
    }

    void Update()
    {
        if (IsSkillMenuActive())
        {
            if (!skillList.activeSelf)
            {
                skillList.SetActive(true);
                CreateSkillList();
            }
        }
        else
        {
            if (skillList.activeSelf)
            {
                // Delete all skill buttons from skillButtonGroup
                foreach (Transform child in skillButtonGroup)
                {
                    GameObject.Destroy(child.gameObject);
                }
                skillList.SetActive(false);
            }

            if (skillDescriptionBox.activeSelf)
            {
                skillDescriptionBox.SetActive(false);
            }
        }
    }

    private bool IsSkillMenuActive()
    {
        return battleMenu.GetCurrentMenuState() == MenuState.SKILL_MENU;
    }

    private void CreateSkillList()
    {
        BattleUnit currentUnit = battleSystem.GetCurrentUnit();
        int numSkills = currentUnit.GetSkillListLength();
        for (int i = 0; i < numSkills; ++i)
        {
            string skillName = currentUnit.GetSkillName(i);
            ActionParams parameters = ActionMasterList.GetParameterObjectByName(skillName);
            if (parameters != null)
            {
                GameObject tempObj = Instantiate(skillSelectButtonPrefab, skillButtonGroup);

                SkillButton tempSkillButton = tempObj.GetComponent<SkillButton>();
                tempSkillButton.SetSkillNameText(parameters.ActionName);
                tempSkillButton.SetMPCostText(parameters.MagicCost);

                Button tempButton = tempObj.GetComponent<Button>();
                bool canUnitAffordCost = currentUnit.Magic >= parameters.MagicCost;
                tempButton.onClick.AddListener(canUnitAffordCost ? delegate { SkillButtonListener(skillName); } : NotEnoughMPListener );
                tempSkillButton.SetTextColor(canUnitAffordCost ? Color.white : Color.gray);

                EventTrigger trigger = tempObj.GetComponent<EventTrigger>();
                
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener(delegate { SkillButtonHoverListener(parameters.ActionDescription); });
                trigger.triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerExit;
                entry2.callback.AddListener(delegate { SkillButtonHoverExitListener(); });
                trigger.triggers.Add(entry2);
            }
        }
    }

    private void SkillButtonListener(string skillName)
    {
        battleSystem.SelectAction(ActionMasterList.GetGenericAttackWithParameterName(skillName));
        battleMenu.SetToTargetMode();
    }

    private void SkillButtonHoverListener(string description)
    {
        if (!skillDescriptionBox.activeSelf)
        {
            skillDescriptionBox.SetActive(true);
        }
        skillDescriptionBoxText.text = description;
    }

    private void SkillButtonHoverExitListener()
    {
        skillDescriptionBox.SetActive(false);
    }

    private void NotEnoughMPListener()
    {
        TextPopups.Announce("Not enough MP!");
    }
}
