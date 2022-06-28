using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        yield return null;

        Debug.Log($"{sourceUnits[0].CharacterName} attacks!");
        yield return new WaitForSeconds(1f);

        int hitRate = BattleCalculator.CalculateHitRate(sourceUnits[0], targetUnits[0], actionParameters);
        if (!BattleCalculator.RollRNG(hitRate))
        {
            Debug.Log("The attack missed!");
            yield return new WaitForSeconds(1f);
            yield break;
        }

        int damage = BattleCalculator.CalculateDamage(sourceUnits[0], targetUnits[0], actionParameters);

        int critRate = BattleCalculator.CalculateCritRate(sourceUnits[0], targetUnits[0], actionParameters);
        if (BattleCalculator.RollRNG(critRate))
        {
            damage *= 2;
            Debug.Log("CRITICAL HIT!!!");
            yield return new WaitForSeconds(1f);
        }

        targetUnits[0].DamageUnit(damage);
        Debug.Log($"{targetUnits[0].CharacterName} took {damage} damage!");
        yield return new WaitForSeconds(1f);

        if (targetUnits[0].IsDead())
        {
            Debug.Log($"{targetUnits[0].CharacterName} was defeated!");
            yield return new WaitForSeconds(1f);
        }
    }
}
