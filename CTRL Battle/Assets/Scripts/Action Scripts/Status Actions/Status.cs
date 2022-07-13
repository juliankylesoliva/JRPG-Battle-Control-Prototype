using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResolveType { START_OF_TURN, END_OF_TURN, START_OF_PHASE, END_OF_PHASE }

public abstract class Status : ActionScript
{
    [SerializeField] protected string statusName = "Status";
    public string StatusName { get { return statusName; } }

    [SerializeField] protected ResolveType resolveDuring = ResolveType.END_OF_TURN;
    public ResolveType ResolveDuring { get { return resolveDuring; } }

    protected BattleUnit targetUnit = null;
    protected int turnsActive = 0;

    protected bool deleteFlag = false;
    public bool DeleteFlag { get { return deleteFlag; } }

    public abstract IEnumerator ApplyStatus();
    public abstract IEnumerator DoStatus();
    public abstract IEnumerator RemoveStatus();

    public void SetTargetUnit(BattleUnit unit)
    {
        targetUnit = unit;
    }

    protected void CreateStatusText(BattleUnit target)
    {
        TextPopups.AddPopupParametersToQueue(new FloatingTextPopupParameters(GetPositionAboveUnit(target), statusName.ToUpper(), Color.red, 6f, 0.5f));
    }

    protected bool RollForAilmentClear(BattleUnit affected)
    {
        return Random.Range(1, 1000) < (int)(affected.Defense * Mathf.Pow(2f, turnsActive));
    }

    protected bool RollForEmotionClear(BattleUnit affected)
    {
        return Random.Range(1, 1000) < (int)(affected.Resistance * Mathf.Pow(2f, turnsActive));
    }

    protected void FlagForDeletion()
    {
        deleteFlag = true;
    }

    new public void StartAction() {/* Disabled */}
    new public void SetSourceUnits(BattleUnit[] units) {/* Disabled */}
    new public void SetTargetUnits(BattleUnit[] units) {/* Disabled */}
    protected override IEnumerator DoAction() { yield return null; } /* Disabled */
}
