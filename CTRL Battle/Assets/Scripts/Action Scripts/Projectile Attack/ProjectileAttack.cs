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
                FloatingTextPopup.Create(GetPositionAboveUnit(targetUnits[0]), "MISS", Color.red, 8f, 1f);
            }
            else
            {
                int damage = actionParameters.Power;
                int critRate = BattleCalculator.CalculateCritRate(sourceUnits[0], targetUnits[0], actionParameters);
                bool crit = BattleCalculator.RollRNG(critRate);
                if (crit)
                {
                    damage *= 2;
                    FloatingTextPopup.Create(GetPositionAboveUnit(targetUnits[0]), "CRITICAL!", Color.blue, 12f, 1f);
                }
                targetUnits[0].DamageUnit(damage);
                totalDamage += damage;

                DamagePopup.Create(GetPositionAboveUnit(targetUnits[0]), damage, crit, 1f);
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

        yield return new WaitForSeconds(1f);
        FloatingTextPopup.Create(targetUnits[0].transform.position + (Vector3.up * 2f), $"{totalDamage} TOTAL", Color.yellow, 12f, 1f);
        yield return new WaitForSeconds(1f);

        if (targetUnits[0].IsDead())
        {
            yield return StartCoroutine(TextPopups.AnnounceForSeconds($"{targetUnits[0].CharacterName} was defeated!", 1f));
        }
    }
}
