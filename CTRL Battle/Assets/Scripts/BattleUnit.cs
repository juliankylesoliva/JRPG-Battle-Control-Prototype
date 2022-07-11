using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    private bool isGuarding = false;
    public bool IsGuarding { get { return isGuarding; } set { isGuarding = value; } }

    [Header("BASIC INFO")]
    [SerializeField] string Name = "Name";
    public string CharacterName
    { 
        get { return Name; }
        set { Name = EnforceCharacterLimit(value, 16); }
    }

    [Header("PHYSICAL STATS")]
    [SerializeField] int CurrentHP = 10;
    public int Health
    {
        get { return CurrentHP; }
        set { CurrentHP = HPMPValueRangeHelper(value, 0, MaxHP); }
    }

    [SerializeField] int MaxHP = 10;
    public int MaxHealth
    {
        get { return MaxHP; }
        set { MaxHP = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int ATK = 1;
    public int Attack
    {
        get { return ATK; }
        set { ATK = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int DEF = 1;
    public int Defense
    {
        get { return DEF; }
        set { DEF = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int AGI = 1;
    public int Agility
    {
        get { return AGI; }
        set { AGI = StatValueRangeHelper(value, 1); }
    }

    [Header("MAGIC STATS")]
    [SerializeField] int CurrentMP = 10;
    public int Magic
    {
        get { return CurrentMP; }
        set { CurrentMP = HPMPValueRangeHelper(value, 0, MaxMP); }
    }

    [SerializeField] int MaxMP = 10;
    public int MaxMagic
    {
        get { return MaxMP; }
        set { MaxMP = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int MAG = 1;
    public int MagicAttack
    {
        get { return MAG; }
        set { MAG = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int RES = 1;
    public int Resistance
    {
        get { return RES; }
        set { RES = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int ACC = 1;
    public int Accuracy
    {
        get { return ACC; }
        set { ACC = StatValueRangeHelper(value, 1); }
    }

    [Header("LIFESTYLE STATS")]
    [SerializeField] int BRV = 1;
    public int Bravery
    {
        get { return BRV; }
        set { BRV = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int CHA = 1;
    public int Charisma
    {
        get { return CHA; }
        set { CHA = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int COM = 1;
    public int Compassion
    {
        get { return COM; }
        set { COM = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int SKL = 1;
    public int Skill
    {
        get { return SKL; }
        set { SKL = StatValueRangeHelper(value, 1); }
    }

    [Header("DAMAGE AFFINITY INFO")]

    [SerializeField] DamageType[] weaknesses;
    [SerializeField] DamageType[] resistances;
    [SerializeField] DamageType[] immunities;
    [SerializeField] DamageType[] absorbances;

    [Header("SKILL INFO")]

    [SerializeField] string meleeAction = "MeleeAttack";
    public string Melee { get { return meleeAction; } }

    [SerializeField] string projectileAction = "ProjectileAttack";
    public string Projectile { get { return projectileAction; } }

    // [SerializeField] string[] equippedSkillActions;

    [Header("PROJECTILE STATS")]
    [SerializeField] int ammoLoaded = 6;
    public int AmmoLoaded
    {
        get { return ammoLoaded; }
        set { ammoLoaded = HPMPValueRangeHelper(value, 0, maxAmmo); }
    }

    [SerializeField] int maxAmmo = 6;
    public int MaxAmmo
    {
        get { return maxAmmo; }
        set { maxAmmo = StatValueRangeHelper(value, 1); }
    }

    [SerializeField] int reloadRate = 1;
    public int ReloadRate
    {
        get { return reloadRate; }
        set { reloadRate = HPMPValueRangeHelper(value, 0, maxAmmo); }
    }

    /* GETTER FUNCTIONS */
    public bool IsDead()
    {
        return CurrentHP <= 0;
    }

    public bool CheckWeakness(DamageType type)
    {
        if (type != DamageType.NONE)
        {
            foreach (DamageType weak in weaknesses)
            {
                if (weak == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckResistance(DamageType type)
    {
        if (type != DamageType.NONE)
        {
            foreach (DamageType resist in resistances)
            {
                if (resist == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckImmunity(DamageType type)
    {
        if (type != DamageType.NONE)
        {
            foreach (DamageType immune in immunities)
            {
                if (immune == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckAbsorbance(DamageType type)
    {
        if (type != DamageType.NONE)
        {
            foreach (DamageType absorb in absorbances)
            {
                if (absorb == type)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /* SETTER FUNCTIONS */
    public void DamageUnit(int damage)
    {
        if (damage <= 0) { return; }
        Health -= damage;
    }

    public void HealUnit(int heal)
    {
        if (heal <= 0) { return; }
        Health += heal;
    }

    public void SpendMagic(int cost)
    {
        if (cost <= 0) { return; }
        Magic -= cost;
    }

    public void RestoreMagic(int restore)
    {
        if (restore <= 0) { return; }
        Magic += restore;
    }

    public void FireAmmo()
    {
        AmmoLoaded--;
    }

    public void ReloadAmmo()
    {
        AmmoLoaded += ReloadRate;
    }

    /* HELPER FUNCTIONS */
    private int HPMPValueRangeHelper(int value, int min, int max)
    {
        if (value < min)
        {
            return min;
        }
        else if (value > max)
        {
            return max;
        }
        else
        {
            return value;
        }
    }

    private int StatValueRangeHelper(int value, int min)
    {
        if (value < min)
        {
            return min;
        }
        else
        {
            return value;
        }
    }

    private string EnforceCharacterLimit(string value, int length)
    {
        if (value.Length > length)
        {
            return value.Substring(0, length);
        }
        else
        {
            return value;
        }
    }
}
