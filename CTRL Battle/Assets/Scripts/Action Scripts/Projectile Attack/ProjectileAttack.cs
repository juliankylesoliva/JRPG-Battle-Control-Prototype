using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : ActionScript
{
    public override IEnumerator DoAction()
    {
        // Prompt controls via announcement
        TextPopups.Announce("[LClk]: Fire! | [LShft]: Cease!");

        // Keep track of player input and total amount of damage.
        bool isPlayerFinished = false;
        int totalDamage = 0;

        do
        {
            // Calculate and evaluate hit rate. Attacks always hit guarding targets
            if (!targetUnits[0].IsGuarding && !BattleCalculator.RollRNG(BattleCalculator.CalculateHitRate(sourceUnits[0], targetUnits[0], actionParameters))) // Miss
            {
                CreateMissText(targetUnits[0]);
            }
            else // Hit
            {
                // Projectile damage is based on Power parameter
                int damage = actionParameters.Power;

                // Check if target is guarding
                bool crit = false;
                if (!targetUnits[0].IsGuarding)
                {
                    // Calculate and evaluate crit rate
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

                // Damage the target unit and add it to the total
                targetUnits[0].DamageUnit(damage);
                totalDamage += damage;
                CreateDamageText(targetUnits[0], damage, crit);
            }

            // Deplete the user's ammo
            sourceUnits[0].FireAmmo();

            // If this unit still has ammo and all selected targets are live, wait for the player's input. If not, break the loop.
            if (sourceUnits[0].AmmoLoaded > 0 && !AreAllTargetsDefeated())
            {
                while (true)
                {
                    yield return null;

                    if (Input.GetKeyDown(KeyCode.LeftShift)) // End the attack
                    {
                        isPlayerFinished = true;
                        break;
                    }

                    if (Input.GetMouseButtonDown(0)) // Fire and continue
                    {
                        break;
                    }
                }
            }
            else
            {
                isPlayerFinished = true;
            }

        } while (!isPlayerFinished);

        // Clear prompt
        TextPopups.ClearPopup();

        // Display total damage
        yield return WaitASec;
        CreateTotalDamageText(targetUnits[0], totalDamage);
        yield return WaitASec;

        if (targetUnits[0].IsDead())
        {
            yield return StartCoroutine(TimedAnnouncement($"{targetUnits[0].CharacterName} was defeated!"));
        }
    }
}
