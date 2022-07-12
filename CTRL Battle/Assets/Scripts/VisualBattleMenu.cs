using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VisualBattleMenu : MonoBehaviour
{
    private BattleMenu battleMenu;
    private BattleSystem battleSystem;

    [SerializeField] GameObject TabKey;
    [SerializeField] TextMeshPro TabText;
    [SerializeField] TextMeshPro ammoText;

    [SerializeField] GameObject LShiftKey;
    [SerializeField] TextMeshPro LShiftText;

    [SerializeField] GameObject SpaceKey;
    [SerializeField] TextMeshPro SpaceText;

    [SerializeField] GameObject WKey;
    [SerializeField] TextMeshPro WText;

    [SerializeField] GameObject AKey;
    [SerializeField] TextMeshPro AText;

    [SerializeField] GameObject SKey;
    [SerializeField] TextMeshPro SText;

    [SerializeField] GameObject DKey;
    [SerializeField] TextMeshPro DText;


    void Awake()
    {
        battleMenu = GameObject.Find("BattleSystem").GetComponent<BattleMenu>();
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }

    void Update()
    {
        MenuState currentMenuState = battleMenu.GetCurrentMenuState();

        if (currentMenuState == MenuState.HOME)
        {
            UseHomeUI();
        }
        else if (currentMenuState == MenuState.GUARD_CONFIRMATION)
        {
            UseGuardUI();
        }
        else if (currentMenuState == MenuState.SKILL_MENU)
        {
            UseSkillUI();
        }
        else if (currentMenuState == MenuState.TARGET_MODE)
        {
            UseTargetUI();
        }
        else
        {
            GoBelowOrigin();
        }
    }

    private void GoInFrontOfCamera()
    {
        this.transform.position = (Camera.main.transform.position + (Camera.main.transform.forward * 4f) + (Vector3.up * -0.5f));
    }

    private void GoBelowOrigin()
    {
        this.transform.position = (Vector3.up * -50f);
    }

    private bool IsUsingProjectileAttack()
    {
        ActionScript selected = battleSystem.GetCurrentAction();
        ActionScript reference = ActionMasterList.GetActionScriptByName("ProjectileAttack");

        if (selected == null || reference == null) { return false; }

        return GameObject.ReferenceEquals(selected.gameObject, reference.gameObject);
    }

    private void UseHomeUI()
    {
        GoInFrontOfCamera();

        TabKey.SetActive(true);
        TabText.gameObject.SetActive(true);
        TabText.text = "SHOOT";
        ammoText.gameObject.SetActive(true);

        LShiftKey.SetActive(true);
        LShiftText.gameObject.SetActive(true);
        LShiftText.text = "GUARD";

        SpaceKey.SetActive(true);
        SpaceText.gameObject.SetActive(true);
        SpaceText.text = "ATTACK";

        WKey.SetActive(true);
        WText.gameObject.SetActive(true);
        AKey.SetActive(true);
        AText.gameObject.SetActive(true);
        SKey.SetActive(true);
        SText.gameObject.SetActive(true);
        DKey.SetActive(true);
        DText.gameObject.SetActive(true);

        BattleUnit unit = battleSystem.GetCurrentUnit();
        ammoText.text = $"{unit.AmmoLoaded}/{unit.MaxAmmo} Left";
    }

    private void UseGuardUI()
    {
        GoInFrontOfCamera();

        TabKey.SetActive(false);
        TabText.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(false);

        LShiftKey.SetActive(true);
        LShiftText.gameObject.SetActive(true);
        LShiftText.text = "BACK";

        SpaceKey.SetActive(true);
        SpaceText.gameObject.SetActive(true);
        SpaceText.text = "CONFIRM";

        WKey.SetActive(false);
        WText.gameObject.SetActive(false);
        AKey.SetActive(false);
        AText.gameObject.SetActive(false);
        SKey.SetActive(false);
        SText.gameObject.SetActive(false);
        DKey.SetActive(false);
        DText.gameObject.SetActive(false);
    }

    private void UseSkillUI()
    {
        TabKey.SetActive(false);
        TabText.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(false);

        LShiftKey.SetActive(true);
        LShiftText.gameObject.SetActive(true);
        LShiftText.text = "BACK";

        SpaceKey.SetActive(false);
        SpaceText.gameObject.SetActive(false);

        WKey.SetActive(false);
        WText.gameObject.SetActive(false);
        AKey.SetActive(false);
        AText.gameObject.SetActive(false);
        SKey.SetActive(false);
        SText.gameObject.SetActive(false);
        DKey.SetActive(false);
        DText.gameObject.SetActive(false);
    }

    private void UseTargetUI()
    {
        GoInFrontOfCamera();

        TabKey.SetActive(false);
        TabText.gameObject.SetActive(false);
        ammoText.gameObject.SetActive(IsUsingProjectileAttack());

        LShiftKey.SetActive(true);
        LShiftText.gameObject.SetActive(true);
        LShiftText.text = "BACK";

        SpaceKey.SetActive(false);
        SpaceText.gameObject.SetActive(false);

        WKey.SetActive(false);
        WText.gameObject.SetActive(false);
        AKey.SetActive(false);
        AText.gameObject.SetActive(false);
        SKey.SetActive(false);
        SText.gameObject.SetActive(false);
        DKey.SetActive(false);
        DText.gameObject.SetActive(false);

        BattleUnit unit = battleSystem.GetCurrentUnit();
        ammoText.text = $"{unit.AmmoLoaded}/{unit.MaxAmmo} Left";
    }
}
