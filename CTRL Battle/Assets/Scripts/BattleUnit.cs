using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    private bool isGuarding = false;
    public bool IsGuarding { get { return isGuarding; } set { isGuarding = value; } }

    [SerializeField] string Name = "Name";
    public string CharacterName
    { 
        get { return Name; }
        set { Name = EnforceCharacterLimit(value, 16); }
    }

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

    /* GETTER FUNCTIONS */
    public bool IsDead()
    {
        return CurrentHP <= 0;
    }

    /* SETTER FUNCTIONS */
    public void DamageUnit(int damage)
    {
        Health -= damage;
    }

    public void HealUnit(int heal)
    {
        Health += heal;
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
