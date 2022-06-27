using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionScript : MonoBehaviour
{
    private BattleSystem battleSystem;

    private bool isWaiting = false;
    private bool isActionCancelled = false;
    private bool isActionConfirmed = false;

    private BattleUnit[] sourceUnits = null;
    private BattleUnit[] targetUnits = null;

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
        isWaiting = true;

        while (!isActionConfirmed)
        {
            isWaiting = true;
            if (isActionCancelled)
            {
                isActionCancelled = false;
                isWaiting = false;
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
}
