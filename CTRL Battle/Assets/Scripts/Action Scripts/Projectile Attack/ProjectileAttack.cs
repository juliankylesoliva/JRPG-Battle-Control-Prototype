using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : ActionScript
{
    protected override IEnumerator DoAction()
    {
        BattleUnit user = sourceUnits[0];
        BattleUnit target = targetUnits[0];

        StartCoroutine(AttackCamera(target));

        // Prompt controls via announcement
        TextPopups.Announce("Fire at will!");


        // Keep track of player input and total amount of damage.
        bool isPlayerFinished = false;
        int totalDamage = 0;
        float beforeHPRatio = GetCurrentHPRatio(target);

        do
        {
            // Calculate and evaluate hit rate. Attacks always hit guarding targets
            if (!BattleCalculator.IsHit(user, target, actionParameters)) // Miss
            {
                CreateMissText(target);
            }
            else // Hit
            {
                // Projectile damage is based on Power parameter
                int damage = actionParameters.Power;

                // Check absorbance
                if (target.CheckAbsorbance(actionParameters.Type))
                {
                    CreateAbsorbText(target);
                    DoHealthRestore(damage, target);
                    break;
                }

                // Check immunity
                if (target.CheckImmunity(actionParameters.Type))
                {
                    CreateNullifyText(target);
                    break;
                }

                // Check for crit
                bool crit = BattleCalculator.IsCrit(user, target, actionParameters);

                // Check weakness hit
                bool weak = target.CheckWeakness(actionParameters.Type);

                // Check resistance hit
                bool resist = target.CheckResistance(actionParameters.Type);

                // Apply damage mods
                ApplyDamageMods(ref damage, crit, weak, resist, user, target);

                // Damage the target unit and add it to the total
                DoAccumulatedDamage(damage, ref totalDamage, crit, target);
            }

            // Deplete the user's ammo
            user.FireAmmo();

            // If this unit still has ammo and all selected targets are live, wait for the player's input. If not, break the loop.
            if (user.AmmoLoaded > 0 && !AreAllTargetsDefeated())
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
        CreateTotalDamageText(target, totalDamage);
        float afterHPRatio = GetCurrentHPRatio(target);
        CreateMeter(target, beforeHPRatio, afterHPRatio, false);
        yield return WaitASec;
    }
}
