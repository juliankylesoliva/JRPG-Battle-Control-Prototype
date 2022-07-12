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
                return (int)((float)parameters.Power * CalculateAttackDefenseRatio(attacker.Attack, defender.Defense) * CalculateDeviation());
            }
        }
        else if (IsMagicDamage(parameters))
        {
            if (IsEarthDamage(parameters))
            {
                return (int)((float)parameters.Power * CalculateAttackDefenseRatio(attacker.MagicAttack, defender.Defense) * CalculateDeviation());
            }
            else
            {
                return (int)((float)parameters.Power * CalculateAttackDefenseRatio(attacker.MagicAttack, defender.Resistance) * CalculateDeviation());
            }
        }
        else { /* Nothing */ }
        return 0;
    }

    private static float CalculateAttackDefenseRatio(int atkAttacker, int defDefender)
    {
        return ((float)atkAttacker) / ((float)defDefender);
    }

    private static float CalculateDeviation()
    {
        return Random.Range(0.85f, 1f);
    }

    public static int CalculateHealing(BattleUnit user, BattleUnit target, ActionParams parameters)
    {
        int resultPercent = (int)((float)parameters.BaseHealingPercentage + ((float)user.Compassion * parameters.HealingPercentageDelta));
        if (resultPercent > parameters.MaxHealingPercentage)
        {
            resultPercent = parameters.MaxHealingPercentage;
        }

        int resultHealing = (int)((float)target.MaxHealth * (((float)resultPercent)/100f));
        if (resultHealing < 1)
        {
            resultHealing = 1;
        }

        return resultHealing;
    }

    public static bool IsHit(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        return attacker.IsGuarding || RollRNG(CalculateHitRate(attacker, defender, parameters));
    }

    private static int CalculateHitRate(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
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

    public static bool IsCrit(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        return !attacker.IsGuarding && (parameters.AlwaysCrits || (parameters.CanCrit && RollRNG(CalculateCritRate(attacker, defender, parameters))));
    }

    private static int CalculateCritRate(BattleUnit attacker, BattleUnit defender, ActionParams parameters)
    {
        int retCritRate = parameters.BaseCritChance;
        int accAgiDiff = (attacker.Skill - defender.Agility);
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

    private static bool RollRNG(int percent)
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
