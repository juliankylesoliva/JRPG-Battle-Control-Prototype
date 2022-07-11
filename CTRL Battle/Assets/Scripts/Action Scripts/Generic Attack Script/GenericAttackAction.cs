using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAttackAction : ActionScript
{
    protected override IEnumerator DoAction()
    {
        BattleUnit user = sourceUnits[0];
        BattleUnit target = targetUnits[0];

        // Attack announcement
        yield return StartCoroutine(AttackCamera(user, 0.5f));
        yield return StartCoroutine(TimedAnnouncement($"{user.CharacterName} used {actionParameters.ActionName}!"));

        // Do a camera change depending on the target mode
        switch (actionParameters.TargetMode)
        {
            case TargetingMode.SINGLE_TEAMMATE:
            case TargetingMode.SINGLE_ENEMY:
                yield return StartCoroutine(AttackCamera(target, 0.5f));
                break;
            case TargetingMode.ALL_TEAMMATE:
                // Check if target is an enemy or not
                if (!battleSystem.IsUnitAnEnemy(target))
                {
                    yield return StartCoroutine(AttackAllPlayersCamera(0.5f));
                }
                else
                {
                    yield return StartCoroutine(AttackAllEnemiesCamera(0.5f));
                }
                break;
            case TargetingMode.ALL_ENEMY:
                // Check if target is an enemy or not
                if (!battleSystem.IsUnitAnEnemy(target))
                {
                    yield return StartCoroutine(AttackAllEnemiesCamera(0.5f));
                }
                else
                {
                    yield return StartCoroutine(AttackAllPlayersCamera(0.5f));
                }
                break;
            case TargetingMode.ALL_UNITS:
                yield return StartCoroutine(AttackAllUnitsCamera(0.5f));
                break;
            case TargetingMode.SELF:
            default:
                yield return StartCoroutine(AttackCamera(user, 0.5f));
                break;
        }

        foreach (BattleUnit currentTarget in targetUnits)
        {
            if (currentTarget != null)
            {
                StartCoroutine(AttackIteration(user, currentTarget));
                yield return new WaitForSeconds(0.4f);
            }
        }

        

        yield return WaitASec;
    }

    private IEnumerator AttackIteration(BattleUnit user, BattleUnit target)
    {
        // Multi-hit option?
        int numHits = (actionParameters.IsMultiHit ? Random.Range(actionParameters.MinHits, actionParameters.MaxHits + 1) : 1);
        int totalDamage = 0;

        for (int i = 0; i < numHits; ++i)
        {
            // Calculate hit rate and roll for hit. This action stops on a miss. Guarding targets cannot avoid attacks.
            if (actionParameters.CheckHitRate && !BattleCalculator.IsHit(user, target, actionParameters))
            {
                CreateMissText(target);
                if (numHits > 1)
                {
                    yield return new WaitForSeconds(0.4f);
                }
                else
                {
                    yield return WaitASec;
                }
            }
            else
            {
                // Calculate initial damage
                int damage = BattleCalculator.CalculateDamage(user, target, actionParameters);
                if (numHits > 1)
                {
                    totalDamage += damage;
                }

                // Calculate crit
                bool crit = BattleCalculator.IsCrit(user, target, actionParameters);

                // Check weakness hit
                bool weak = target.CheckWeakness(actionParameters.Type);

                // Check resistance hit
                bool resist = target.CheckResistance(actionParameters.Type);

                // Apply any applicable damage modifiers
                ApplyDamageMods(ref damage, crit, weak, resist, user, target);

                // Apply the calculated damage to the target unit
                DoSingleHitDamage(damage, crit, target);
            }
        }

        if (numHits > 1)
        {
            yield return WaitASec;
            CreateTotalDamageText(target, totalDamage);
        }
    }
}
