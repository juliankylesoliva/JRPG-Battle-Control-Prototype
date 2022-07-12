using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { BASH, SLICE, PIERCE, PROJECTILE, FIRE, WATER, EARTH, WIND, ICE, ELECTRIC, HEALING, EMOTION, AILMENT, STATUS, NONE }

[CreateAssetMenu(fileName = "ActionParams", menuName = "ScriptableObjects/ActionParams", order = 1)]
public class ActionParams : ScriptableObject
{
    [Header("General Parameters")]
    [SerializeField] string actionName = "Action Name";
    public string ActionName { get { return actionName; } }

    [SerializeField] int magicCost = 0;
    public int MagicCost { get { return magicCost; } }

    [SerializeField] bool useActionNameAsMessage = false;
    public bool UseActionNameAsMessage { get { return useActionNameAsMessage; } }

    [SerializeField] string actionNameInMessage = "";
    public string ActionNameInMessage { get { return actionNameInMessage; } }

    [SerializeField] string actionVerb = "used";
    public string ActionVerb { get { return actionVerb; } }

    [SerializeField] DamageType damageType = DamageType.NONE;
    public DamageType Type { get { return damageType; } }

    [SerializeField] TargetingMode targetMode = TargetingMode.NONE;
    public TargetingMode TargetMode { get { return targetMode; } }

    [SerializeField] UnitTargetStatus targetStatus = UnitTargetStatus.EITHER;
    public UnitTargetStatus TargetStatus { get { return targetStatus; } }

    [Header("Attack Parameters")]
    [SerializeField, Range(0, 200)] int power = 10;
    public int Power { get { return power; } }

    [SerializeField] bool checkHitRate = true;
    public bool CheckHitRate { get { return checkHitRate; } }

    [SerializeField, Range(0, 100)] int basePrecision = 90;
    public int BasePrecision { get { return basePrecision; } }

    [SerializeField, Range(0f, 1f)] float precisionDelta = 0.1f;
    public float PrecisionDelta { get { return precisionDelta; } }

    [SerializeField] bool alwaysCrits = false;
    public bool AlwaysCrits { get { return alwaysCrits; } }

    [SerializeField] bool canCrit = true;
    public bool CanCrit { get { return canCrit; } }

    [SerializeField, Range(0, 100)] int baseCritChance = 5;
    public int BaseCritChance { get { return baseCritChance; } }

    [SerializeField, Range(0f, 1f)] float critChanceDelta = 0.05f;
    public float CritChanceDelta { get { return critChanceDelta; } }

    [SerializeField] bool isMultiHit = false;
    public bool IsMultiHit { get { return isMultiHit; } }

    [SerializeField, Range(1, 8)] int minHits = 1;
    public int MinHits { get { return minHits; } }

    [SerializeField, Range(1, 8)] int maxHits = 1;
    public int MaxHits { get { return maxHits; } }

    [SerializeField] bool applyStatusOnHit = false;
    public bool ApplyStatusOnHit { get { return applyStatusOnHit; } }

    [SerializeField] string statusNameToApply = "None";
    public string StatusNameToApply { get { return statusNameToApply; } }

    [SerializeField, Range(0, 100)] int statusChancePercent = 10;
    public int StatusChancePercent { get { return statusChancePercent; } }

    [Header("Healing Parameters")]
    [SerializeField, Range(0, 100)] int baseHealingPercentage = 20;
    public int BaseHealingPercentage { get { return baseHealingPercentage; } }

    [SerializeField, Range(0, 100)] int maxHealingPercentage = 25;
    public int MaxHealingPercentage { get { return maxHealingPercentage; } }

    [SerializeField, Range(0f, 1f)] float healingPercentageDelta = 0.1f;
    public float HealingPercentageDelta { get { return healingPercentageDelta; } }
}
