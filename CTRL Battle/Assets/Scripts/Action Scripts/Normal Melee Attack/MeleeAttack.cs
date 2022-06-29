using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        yield return null;

        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{sourceUnits[0].CharacterName} attacks!", 1f));

        int hitRate = BattleCalculator.CalculateHitRate(sourceUnits[0], targetUnits[0], actionParameters);
        if (!BattleCalculator.RollRNG(hitRate))
        {
            yield return StartCoroutine(TextPopups.AnnounceForSeconds("The attack missed!", 1f));
            yield break;
        }

        int damage = BattleCalculator.CalculateDamage(sourceUnits[0], targetUnits[0], actionParameters);

        int critRate = BattleCalculator.CalculateCritRate(sourceUnits[0], targetUnits[0], actionParameters);
        bool crit = BattleCalculator.RollRNG(critRate);
        if (crit)
        {
            damage *= 2;
        }

        targetUnits[0].DamageUnit(damage);
        DamagePopup.Create(targetUnits[0].transform.position + (Vector3.up * 2f), damage, crit, 1f);
        yield return new WaitForSeconds(1f);

        if (targetUnits[0].IsDead())
        {
            yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnits[0].CharacterName} was defeated!", 1f));
        }
    }
}
