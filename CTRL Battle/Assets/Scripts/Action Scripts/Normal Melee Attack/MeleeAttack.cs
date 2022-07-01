using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        // Attack announcement
        yield return StartCoroutine(AttackCamera(sourceUnits[0], 0.5f));
        yield return StartCoroutine(TimedAnnouncement($"{sourceUnits[0].CharacterName} attacks!"));
        yield return StartCoroutine(AttackCamera(targetUnits[0], 0.5f));

        // Calculate hit rate and roll for hit. This action stops on a miss. Guarding targets cannot avoid attacks.
        if (!targetUnits[0].IsGuarding && !BattleCalculator.RollRNG(BattleCalculator.CalculateHitRate(sourceUnits[0], targetUnits[0], actionParameters)))
        {
            CreateMissText(targetUnits[0]);
            yield return WaitASec;
            yield break;
        }

        // Calculate initial damage
        int damage = BattleCalculator.CalculateDamage(sourceUnits[0], targetUnits[0], actionParameters);

        // Check if the target is guarding. Cannot land crits or hit weaknesses while guarding.
        bool crit = false;
        if (!targetUnits[0].IsGuarding)
        {
            // Calculate crit rate and roll for crit. Double the above damage variable if successful.
            int critRate = BattleCalculator.CalculateCritRate(sourceUnits[0], targetUnits[0], actionParameters);
            crit = BattleCalculator.RollRNG(critRate);
            if (crit)
            {
                damage *= 2;
                CreateCritText(targetUnits[0]);
            }
        }
        else
        {
            damage /= 2;
            CreateGuardText(targetUnits[0]);
        }

        // Apply the calculated damage to the target unit
        float beforeHPRatio = GetCurrentHPRatio(targetUnits[0]);
        targetUnits[0].DamageUnit(damage);
        float afterHPRatio = GetCurrentHPRatio(targetUnits[0]);
        CreateMeter(targetUnits[0], beforeHPRatio, afterHPRatio, false);
        CreateDamageText(targetUnits[0], damage, crit);
        if (targetUnits[0].IsDead())
        {
            CreateKOdText(targetUnits[0]);
        }
        yield return WaitASec;
    }
}
