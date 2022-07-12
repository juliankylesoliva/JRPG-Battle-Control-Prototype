using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Status
{
    public override IEnumerator ApplyStatus()
    {
        if (targetUnit == null) { yield break; }
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName} got burned!", 1f));
    }

    public override IEnumerator DoStatus()
    {
        if (targetUnit == null) { yield break; }
        yield return StartCoroutine(AttackCamera(targetUnit, 0.5f));

        int damage = (int)(targetUnit.Health * 0.15f);
        if (damage < 1) { damage = 1; }
        DoSingleHitDamage(damage, false, targetUnit);
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName} was hurt by the burn!", 1f));
    }

    public override IEnumerator RemoveStatus()
    {
        if (targetUnit == null) { yield break; }
        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnit.CharacterName}'s burn healed!", 1f));
    }
}
