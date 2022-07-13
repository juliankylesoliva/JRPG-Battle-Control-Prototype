using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Status
{
    public override IEnumerator ApplyStatus()
    {
        if (deleteFlag || targetUnit == null) { yield break; }
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName} got burned!", 1f));
    }

    public override IEnumerator DoStatus()
    {
        if (deleteFlag || targetUnit == null) { yield break; }
        yield return StartCoroutine(AttackCamera(targetUnit, 0.5f));

        int damage = targetUnit.GetPercentOfCurrentHP(15f);
        DoSingleHitDamage(damage, false, targetUnit);
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName} was hurt by the burn!", 1f));

        if (RollForAilmentClear(targetUnit))
        {
            yield return StartCoroutine(targetUnit.RemoveStatus(statusName));
        }
        else
        {
            turnsActive++;
        }
    }

    public override IEnumerator RemoveStatus()
    {
        if (deleteFlag || targetUnit == null) { yield break; }
        yield return StartCoroutine(AttackCamera(targetUnit, 0.5f));
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName}'s burn healed!", 1f));
        FlagForDeletion();
    }
}
