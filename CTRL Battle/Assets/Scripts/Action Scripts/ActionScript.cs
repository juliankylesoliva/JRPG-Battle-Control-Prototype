using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionScript : MonoBehaviour
{
    public ActionParams actionParameters;

    private BattleSystem battleSystem;

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

    public abstract IEnumerator DoAction();

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
        return (unit.transform.position + (Vector3.up * 2f));
    }
}
