using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionScript : MonoBehaviour
{
    public ActionParams actionParameters;

    protected BattleSystem battleSystem;

    private bool isWaiting = false;
    private bool isActionCancelled = false;
    private bool isActionConfirmed = false;

    protected BattleUnit[] sourceUnits = null;
    protected BattleUnit[] targetUnits = null;

    void Awake()
    {
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }

    public void InitiateAction()
    {
        StartCoroutine(WaitForConfirmation());
    }

    public void CancelAction()
    {
        if (isWaiting && !isActionCancelled && !isActionConfirmed) { isActionCancelled = true; }
    }

    public void ConfirmAction()
    {
        if (isWaiting && !isActionCancelled && !isActionConfirmed) { isActionConfirmed = true; }
    }

    public void SetSourceUnits(BattleUnit[] units)
    {
        sourceUnits = units;
    }

    public void SetTargetUnits(BattleUnit[] units)
    {
        targetUnits = units;
    }

    private IEnumerator WaitForConfirmation()
    {
        isActionCancelled = false;
        isActionConfirmed = false;
        sourceUnits = null;
        targetUnits = null;
        isWaiting = true;

        while (!isActionConfirmed)
        {
            isWaiting = true;
            if (isActionCancelled)
            {
                isActionCancelled = false;
                isWaiting = false;
                sourceUnits = null;
                targetUnits = null;
                yield break;
            }
            else
            {
                yield return null;
            }
        }

        isActionConfirmed = false;
        isWaiting = false;
        yield return StartCoroutine(DoAction());
        sourceUnits = null;
        targetUnits = null;
        battleSystem.EndOfTurn();
    }

    public abstract IEnumerator DoAction(); // Create a new script that inherits this class and override this function.

    protected WaitForSeconds WaitASec = new WaitForSeconds(1f);

    protected bool AreAllTargetsDefeated()
    {
        foreach (BattleUnit unit in targetUnits)
        {
            if (unit != null && !unit.IsDead()) { return false; }
        }
        return true;
    }

    protected Vector3 GetPositionAboveUnit(BattleUnit unit)
    {
        return (unit.transform.position + (Vector3.up * 1.5f));
    }

    protected void CreateMissText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "MISS", Color.red, 6f, 1f);
    }

    protected void CreateGuardText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "GUARD", Color.gray, 6f, 1f);
    }

    protected void CreateCritText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "CRITICAL!", Color.blue, 9f, 1f);
    }

    protected void CreateKOdText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "KO'd!", Color.red, 9f, 1f);
    }

    protected void CreateTotalDamageText(BattleUnit target, int damage)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), $"{damage} TOTAL", Color.yellow, 9f, 1f);
    }

    protected void CreateDamageText(BattleUnit target, int damage, bool isCrit)
    {
        DamagePopup.Create(GetPositionAboveUnit(target), damage, isCrit, 1f);
    }

    protected IEnumerator TimedAnnouncement(string message)
    {
        yield return StartCoroutine(TextPopups.AnnounceForSeconds(message, 1f));
    }

    protected IEnumerator AttackCamera(BattleUnit unit, float time = 0f)
    {
        CameraSwitcher.ActionCamera(battleSystem.GetSlotCodeFromUnit(unit));
        yield return new WaitForSeconds(time);
    }
}
