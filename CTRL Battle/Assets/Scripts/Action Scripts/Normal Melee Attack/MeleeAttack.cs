using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        BattleUnit user = sourceUnits[0];
        BattleUnit target = targetUnits[0];

        // Attack announcement
        yield return StartCoroutine(AttackCamera(user, 0.5f));
        yield return StartCoroutine(TimedAnnouncement($"{user.CharacterName} attacks!"));
        yield return StartCoroutine(AttackCamera(target, 0.5f));

        // Calculate hit rate and roll for hit. This action stops on a miss. Guarding targets cannot avoid attacks.
        if (!BattleCalculator.IsHit(user, target, actionParameters))
        {
            CreateMissText(target);
            yield return WaitASec;
            yield break;
        }

        // Calculate initial damage
        int damage = BattleCalculator.CalculateDamage(user, target, actionParameters);

        // Calculate crit
        bool crit = BattleCalculator.IsCrit(user, target, actionParameters);

        // Apply any applicable damage modifiers
        ApplyDamageMods(ref damage, crit, user, target);

        // Apply the calculated damage to the target unit
        DoSingleHitDamage(damage, crit, target);

        yield return WaitASec;
    }
}
