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

    public abstract IEnumerator ApplyStatus();
    public abstract IEnumerator DoStatus();
    public abstract IEnumerator RemoveStatus();

    public void SetTargetUnit(BattleUnit unit)
    {
        targetUnit = unit;
    }

    protected override IEnumerator DoAction() { yield return null; }
}
