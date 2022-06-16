using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    /* VARIABLES */
    const int HP_BASE_MULTIPLIER = 10;
    const int ATK_BASE_MULTIPLIER = 5;
    const int DEF_BASE_MULTIPLIER = 4;
    const int SPD_BASE_MULTIPLIER = 3;
    const int MP_BASE_MULTIPLIER = 10;
    const int MAG_BASE_MULTIPLIER = 5;
    const int RES_BASE_MULTIPLIER = 4;
    const int ACC_BASE_MULTIPLIER = 3;
    const int BRV_BASE_MULTIPLIER = 1;
    const int CHA_BASE_MULTIPLIER = 1;
    const int COM_BASE_MULTIPLIER = 1;
    const int SKL_BASE_MULTIPLIER = 1;

    const int STAT_BONUS_DIVIDER = 5;

    [Header("BASIC INFO")]
    [SerializeField] string characterName = "Name";
    [SerializeField, Range(1, 100)] int characterLevel = 1;

    [Header("BASE STATS INFO")]
    [SerializeField, Range(1, 20)] int HP_Base  = 10;
    [SerializeField, Range(1, 20)] int ATK_Base = 10;
    [SerializeField, Range(1, 20)] int DEF_Base = 10;
    [SerializeField, Range(1, 20)] int SPD_Base = 10;
    [SerializeField, Range(1, 20)] int MP_Base  = 10;
    [SerializeField, Range(1, 20)] int MAG_Base = 10;
    [SerializeField, Range(1, 20)] int RES_Base = 10;
    [SerializeField, Range(1, 20)] int ACC_Base = 10;
    [SerializeField, Range(1, 20)] int BRV_Base = 10;
    [SerializeField, Range(1, 20)] int CHA_Base = 10;
    [SerializeField, Range(1, 20)] int COM_Base = 10;
    [SerializeField, Range(1, 20)] int SKL_Base = 10;

    [Header("STAT GROWTH RATES")]
    [SerializeField, Range(0f, 5f)] float HP_Growth  = 3f;
    [SerializeField, Range(0f, 5f)] float ATK_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float DEF_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float SPD_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float MP_Growth  = 3f;
    [SerializeField, Range(0f, 5f)] float MAG_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float RES_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float ACC_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float BRV_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float CHA_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float COM_Growth = 3f;
    [SerializeField, Range(0f, 5f)] float SKL_Growth = 3f;

    [Header("STAT BONUSES")]
    [SerializeField, Range(0, 250)] int HP_Bonus  = 0;
    [SerializeField, Range(0, 250)] int ATK_Bonus = 0;
    [SerializeField, Range(0, 250)] int DEF_Bonus = 0;
    [SerializeField, Range(0, 250)] int SPD_Bonus = 0;
    [SerializeField, Range(0, 250)] int MP_Bonus  = 0;
    [SerializeField, Range(0, 250)] int MAG_Bonus = 0;
    [SerializeField, Range(0, 250)] int RES_Bonus = 0;
    [SerializeField, Range(0, 250)] int ACC_Bonus = 0;
    [SerializeField, Range(0, 250)] int BRV_Bonus = 0;
    [SerializeField, Range(0, 250)] int CHA_Bonus = 0;
    [SerializeField, Range(0, 250)] int COM_Bonus = 0;
    [SerializeField, Range(0, 250)] int SKL_Bonus = 0;

    [Header("UNMODIFIED STAT VALUES")]
    [SerializeField] int HP;
    [SerializeField] int ATK;
    [SerializeField] int DEF;
    [SerializeField] int SPD;
    [SerializeField] int MP;
    [SerializeField] int MAG;
    [SerializeField] int RES;
    [SerializeField] int ACC;
    [SerializeField] int BRV;
    [SerializeField] int CHA;
    [SerializeField] int COM;
    [SerializeField] int SKL;

    void Update()
    {
        UpdateUnmodifiedStatValues();
    }

    /* HELPER FUNCTIONS */
    private int GetUnmodifiedStatValue(int baseStat, int baseMultiplier, float growthRate, int statBonus)
    {
        return (baseStat * baseMultiplier) + (int)(growthRate * (characterLevel - 1)) + (statBonus / STAT_BONUS_DIVIDER);
    }

    private void UpdateUnmodifiedStatValues()
    {
        HP  = GetUnmodifiedStatValue(HP_Base, HP_BASE_MULTIPLIER, HP_Growth, HP_Bonus);
        ATK = GetUnmodifiedStatValue(ATK_Base, ATK_BASE_MULTIPLIER, ATK_Growth, ATK_Bonus);
        DEF = GetUnmodifiedStatValue(DEF_Base, DEF_BASE_MULTIPLIER, DEF_Growth, DEF_Bonus);
        SPD = GetUnmodifiedStatValue(SPD_Base, SPD_BASE_MULTIPLIER, SPD_Growth, SPD_Bonus);
        MP  = GetUnmodifiedStatValue(MP_Base, MP_BASE_MULTIPLIER, MP_Growth, MP_Bonus);
        MAG = GetUnmodifiedStatValue(MAG_Base, MAG_BASE_MULTIPLIER, MAG_Growth, MAG_Bonus);
        RES = GetUnmodifiedStatValue(RES_Base, RES_BASE_MULTIPLIER, RES_Growth, RES_Bonus);
        ACC = GetUnmodifiedStatValue(ACC_Base, ACC_BASE_MULTIPLIER, ACC_Growth, ACC_Bonus);
        BRV = GetUnmodifiedStatValue(BRV_Base, BRV_BASE_MULTIPLIER, BRV_Growth, BRV_Bonus);
        CHA = GetUnmodifiedStatValue(CHA_Base, CHA_BASE_MULTIPLIER, CHA_Growth, CHA_Bonus);
        COM = GetUnmodifiedStatValue(COM_Base, COM_BASE_MULTIPLIER, COM_Growth, COM_Bonus);
        SKL = GetUnmodifiedStatValue(SKL_Base, SKL_BASE_MULTIPLIER, SKL_Growth, SKL_Bonus);
    }
}
