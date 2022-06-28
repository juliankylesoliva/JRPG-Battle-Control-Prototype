using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalculator : MonoBehaviour
{
    public static int CalculateDamage(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        if (IsPhysicalDamage(parameters))
        {
            if (IsProjectileDamage(parameters))
            {
                return parameters.Power;
            }
            else
            {
                return (int)((float)parameters.Power * CalculateAttackDefenseRatio(attacker.Attack, defender.Defense));
            }
        }
        else if (IsMagicDamage(parameters))
        {
            if (IsEarthDamage(parameters))
            {
                
            }
            else
            {

            }
        }
        else { /* Nothing */ }
        return 0;
    }

    public static float CalculateAttackDefenseRatio(int atkAttacker, int defDefender)
    {
        return ((float)atkAttacker) / ((float)defDefender);
    }

    public static int CalculateHitRate(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        int retHitRate = parameters.BasePrecision;
        int accAgiDiff = (attacker.Accuracy - defender.Agility);
        int totalDelta = (int)(parameters.PrecisionDelta * (float)accAgiDiff);
        retHitRate += totalDelta;

        if (retHitRate > 100)
        {
            return 100;
        }
        else if (retHitRate < 0)
        {
            return 0;
        }
        else
        {
            return retHitRate;
        }
    }

    public static int CalculateCritRate(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        int retCritRate = parameters.BaseCritChance;
        int accAgiDiff = (attacker.Accuracy - defender.Agility);
        int totalDelta = (int)(parameters.CritChanceDelta * (float)accAgiDiff);
        retCritRate += totalDelta;

        if (retCritRate > 100)
        {
            return 100;
        }
        else if (retCritRate < 0)
        {
            return 0;
        }
        else
        {
            return retCritRate;
        }
    }

    public static bool RollRNG(int percent)
    {
        return Random.Range(0, 100) < percent;
    }

    /* HELPER FUNCTIONS */
    private static bool IsPhysicalDamage(ActionParams parameters)
    {
        DamageType[] physList = new DamageType[] { DamageType.BASH, DamageType.SLICE, DamageType.PIERCE, DamageType.PROJECTILE };
        foreach (DamageType type in physList)
        {
            if (type == parameters.Type)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsProjectileDamage(ActionParams parameters)
    {
        return parameters.Type == DamageType.PROJECTILE;
    }

    private static bool IsMagicDamage(ActionParams parameters)
    {
        DamageType[] physList = new DamageType[] { DamageType.FIRE, DamageType.WATER, DamageType.EARTH, DamageType.WIND, DamageType.ICE, DamageType.ELECTRIC };
        foreach (DamageType type in physList)
        {
            if (type == parameters.Type)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsEarthDamage(ActionParams parameters)
    {
        return parameters.Type == DamageType.EARTH;
    }
}