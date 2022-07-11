using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { BASH, SLICE, PIERCE, PROJECTILE, FIRE, WATER, EARTH, WIND, ICE, ELECTRIC, NONE }

[CreateAssetMenu(fileName = "ActionParams", menuName = "ScriptableObjects/ActionParams", order = 1)]
public class ActionParams : ScriptableObject
{
    [SerializeField] string actionName = "Action Name";
    public string ActionName { get { return actionName; } }

    [SerializeField] DamageType damageType = DamageType.NONE;
    public DamageType Type { get { return damageType; } }

    [SerializeField] TargetingMode targetMode = TargetingMode.NONE;
    public TargetingMode TargetMode { get { return targetMode; } }

    [SerializeField] UnitTargetStatus targetStatus = UnitTargetStatus.EITHER;
    public UnitTargetStatus TargetStatus { get { return targetStatus; } }

    [SerializeField, Range(0, 100)] int power = 10;
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
}
