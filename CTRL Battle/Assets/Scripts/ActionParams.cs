using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { NONE, BASH, SLICE, PIERCE, PROJECTILE, FIRE, WATER, EARTH, WIND }

[CreateAssetMenu(fileName = "ActionParams", menuName = "ScriptableObjects/ActionParams", order = 1)]
public class ActionParams : ScriptableObject
{
    [SerializeField] DamageType primaryType = DamageType.NONE;
    public DamageType PrimaryType { get { return primaryType; } }

    [SerializeField] DamageType secondaryType = DamageType.NONE;
    public DamageType SecondaryType { get { return secondaryType; } }

    [SerializeField] TargetingMode targetMode = TargetingMode.NONE;
    public TargetingMode TargetMode { get { return targetMode; } }

    [SerializeField, Range(0, 100)] int power = 10;
    public int Power { get { return power; } }

    [SerializeField, Range(0, 100)] int basePrecision = 90;
    public int BasePrecision { get { return basePrecision; } }

    [SerializeField, Range(0f, 1f)] float precisionDelta = 0.1f;
    public float PrecisionDelta { get { return precisionDelta; } }
}
