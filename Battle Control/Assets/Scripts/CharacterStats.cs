using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Stat { HP, ATK, DEF, SPD, MP, MAG, RES, ACC, BRV, CHA, COM, SKL }

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
    const int STAT_CHANGE_CONSTANT = 3;

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

    [Header("CURRENT STAT VALUES")]
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

    [Header("STAT CHANGE STAGES")]
    [SerializeField, Range(-3, 3)] int ATK_Stage = 0;
    [SerializeField, Range(-3, 3)] int DEF_Stage = 0;
    [SerializeField, Range(-3, 3)] int SPD_Stage = 0;
    [SerializeField, Range(-3, 3)] int MAG_Stage = 0;
    [SerializeField, Range(-3, 3)] int RES_Stage = 0;
    [SerializeField, Range(-3, 3)] int ACC_Stage = 0;
    [SerializeField, Range(-3, 3)] int BRV_Stage = 0;
    [SerializeField, Range(-3, 3)] int CHA_Stage = 0;
    [SerializeField, Range(-3, 3)] int COM_Stage = 0;
    [SerializeField, Range(-3, 3)] int SKL_Stage = 0;

    void Update()
    {
        UpdateAllStats();
    }

    /* HELPER FUNCTIONS */
    private int CalculateStatFromCharacteristics(int baseStat, int baseMultiplier, float growthRate, int statBonus)
    {
        return (baseStat * baseMultiplier) + (int)(growthRate * (characterLevel - 1)) + (statBonus / STAT_BONUS_DIVIDER);
    }

    private int CalculateStatFromBattleModifiers(int unmodded, int stage = 0)
    {
        int retVal = (int)(unmodded * GetStatStageMultiplier(stage));
        return (retVal > 0 ? retVal : 1);
    }

    private float GetStatStageMultiplier(int stage)
    {
        if (stage == 0) { return 1f; }
        return (stage > 0 ? (float)(STAT_CHANGE_CONSTANT + stage) / (float)STAT_CHANGE_CONSTANT : (float)STAT_CHANGE_CONSTANT / (float)(STAT_CHANGE_CONSTANT - stage));
    }

    private void UpdateStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.HP:
                HP = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.HP));
                break;
            case Stat.ATK:
                ATK = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.ATK), ATK_Stage);
                break;
            case Stat.DEF:
                DEF = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.DEF), DEF_Stage);
                break;
            case Stat.SPD:
                SPD = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.SPD), SPD_Stage);
                break;
            case Stat.MP:
                MP = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.MP));
                break;
            case Stat.MAG:
                MAG = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.MAG), MAG_Stage);
                break;
            case Stat.RES:
                RES = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.RES), RES_Stage);
                break;
            case Stat.ACC:
                ACC = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.ACC), ACC_Stage);
                break;
            case Stat.BRV:
                BRV = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.BRV), BRV_Stage);
                break;
            case Stat.CHA:
                CHA = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.CHA), CHA_Stage);
                break;
            case Stat.COM:
                COM = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.COM), COM_Stage);
                break;
            case Stat.SKL:
                SKL = CalculateStatFromBattleModifiers(GetUnmoddedStat(Stat.SKL), SKL_Stage);
                break;
            default:
                break;
        }
    }

    private void UpdateAllStats()
    {
        UpdateStat(Stat.HP);
        UpdateStat(Stat.ATK);
        UpdateStat(Stat.DEF);
        UpdateStat(Stat.SPD);
        UpdateStat(Stat.MP);
        UpdateStat(Stat.MAG);
        UpdateStat(Stat.RES);
        UpdateStat(Stat.ACC);
        UpdateStat(Stat.BRV);
        UpdateStat(Stat.CHA);
        UpdateStat(Stat.COM);
        UpdateStat(Stat.SKL);
    }

    /* PUBLIC METHODS */
    public int GetUnmoddedStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.HP:
                return CalculateStatFromCharacteristics(HP_Base, HP_BASE_MULTIPLIER, HP_Growth, HP_Bonus);
            case Stat.ATK:
                return CalculateStatFromCharacteristics(ATK_Base, ATK_BASE_MULTIPLIER, ATK_Growth, ATK_Bonus);
            case Stat.DEF:
                return CalculateStatFromCharacteristics(DEF_Base, DEF_BASE_MULTIPLIER, DEF_Growth, DEF_Bonus);
            case Stat.SPD:
                return CalculateStatFromCharacteristics(SPD_Base, SPD_BASE_MULTIPLIER, SPD_Growth, SPD_Bonus);
            case Stat.MP:
                return CalculateStatFromCharacteristics(MP_Base, MP_BASE_MULTIPLIER, MP_Growth, MP_Bonus);
            case Stat.MAG:
                return CalculateStatFromCharacteristics(MAG_Base, MAG_BASE_MULTIPLIER, MAG_Growth, MAG_Bonus);
            case Stat.RES:
                return CalculateStatFromCharacteristics(RES_Base, RES_BASE_MULTIPLIER, RES_Growth, RES_Bonus);
            case Stat.ACC:
                return CalculateStatFromCharacteristics(ACC_Base, ACC_BASE_MULTIPLIER, ACC_Growth, ACC_Bonus);
            case Stat.BRV:
                return CalculateStatFromCharacteristics(BRV_Base, BRV_BASE_MULTIPLIER, BRV_Growth, BRV_Bonus);
            case Stat.CHA:
                return CalculateStatFromCharacteristics(CHA_Base, CHA_BASE_MULTIPLIER, CHA_Growth, CHA_Bonus);
            case Stat.COM:
                return CalculateStatFromCharacteristics(COM_Base, COM_BASE_MULTIPLIER, COM_Growth, COM_Bonus);
            case Stat.SKL:
                return CalculateStatFromCharacteristics(SKL_Base, SKL_BASE_MULTIPLIER, SKL_Growth, SKL_Bonus);
            default:
                return -1;
        }
    }

    public int GetBattleStat(Stat stat)
    {
        switch (stat)
        {
            case Stat.HP:
                UpdateStat(Stat.HP);
                return HP;
            case Stat.ATK:
                UpdateStat(Stat.ATK);
                return ATK;
            case Stat.DEF:
                UpdateStat(Stat.DEF);
                return DEF;
            case Stat.SPD:
                UpdateStat(Stat.SPD);
                return SPD;
            case Stat.MP:
                UpdateStat(Stat.MP);
                return MP;
            case Stat.MAG:
                UpdateStat(Stat.MAG);
                return MAG;
            case Stat.RES:
                UpdateStat(Stat.RES);
                return RES;
            case Stat.ACC:
                UpdateStat(Stat.ACC);
                return ACC;
            case Stat.BRV:
                UpdateStat(Stat.BRV);
                return BRV;
            case Stat.CHA:
                UpdateStat(Stat.CHA);
                return CHA;
            case Stat.COM:
                UpdateStat(Stat.COM);
                return COM;
            case Stat.SKL:
                UpdateStat(Stat.SKL);
                return SKL;
            default:
                return -1;
        }
    }
}
