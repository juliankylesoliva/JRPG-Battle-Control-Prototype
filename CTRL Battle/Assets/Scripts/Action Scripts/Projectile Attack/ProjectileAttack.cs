using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        yield return null;

        bool isPlayerFinished = false;
        int totalDamage = 0;

        do
        {
            int hitRate = BattleCalculator.CalculateHitRate(sourceUnits[0], targetUnits[0], actionParameters);
            if (!BattleCalculator.RollRNG(hitRate))
            {
                TextPopups.Announce($"Missed! ({totalDamage} total damage)");
            }
            else
            {
                int damage = actionParameters.Power;
                int critRate = BattleCalculator.CalculateCritRate(sourceUnits[0], targetUnits[0], actionParameters);
                bool crit = BattleCalculator.RollRNG(critRate);
                if (crit)
                {
                    damage *= 2;
                }
                targetUnits[0].DamageUnit(damage);
                totalDamage += damage;

                DamagePopup.Create(targetUnits[0].transform.position + (Vector3.up * 2f), damage, crit, 1f);
                TextPopups.Announce($"{(crit ? "CRIT!!!" : "Hit!")} ({totalDamage} total damage)");
            }

            sourceUnits[0].FireAmmo();

            while (sourceUnits[0].AmmoLoaded > 0 && !AreAllTargetsDefeated())
            {
                yield return null;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    isPlayerFinished = true;
                    break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
            }

        } while (!isPlayerFinished && sourceUnits[0].AmmoLoaded > 0 && !AreAllTargetsDefeated());

        yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnits[0].CharacterName} took {totalDamage} total damage!", 1f));
        if (targetUnits[0].IsDead())
        {
            yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnits[0].CharacterName} was defeated!", 1f));
        }
    }
}
