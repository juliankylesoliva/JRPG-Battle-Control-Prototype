using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionScript : MonoBehaviour
{
    public ActionParams actionParameters;

    protected BattleSystem battleSystem;

    protected BattleUnit[] sourceUnits = null;
    protected BattleUnit[] targetUnits = null;

    void Awake()
    {
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
    }

    public void StartAction()
    {
        StartCoroutine(InitiateAction());
    }

    public void SetSourceUnits(BattleUnit[] units)
    {
        sourceUnits = units;
    }

    public void SetTargetUnits(BattleUnit[] units)
    {
        targetUnits = units;
    }

    private IEnumerator InitiateAction()
    {
        yield return StartCoroutine(DoAction());
        sourceUnits = null;
        targetUnits = null;
        battleSystem.EndOfTurn();
    }

    protected abstract IEnumerator DoAction(); // Create a new script that inherits this class and override this function.

    protected WaitForSeconds WaitASec = new WaitForSeconds(1f);

    protected bool AreAllTargetsDefeated()
    {
        foreach (BattleUnit unit in targetUnits)
        {
            if (unit != null && !unit.IsDead()) { return false; }
        }
        return true;
    }

    protected float GetCurrentHPRatio(BattleUnit unit)
    {
        return (((float)unit.Health) / ((float)unit.MaxHealth));
    }

    protected float GetCurrentMPRation(BattleUnit unit)
    {
        return (((float)unit.Magic) / ((float)unit.MaxMagic));
    }

    protected Vector3 GetPositionAboveUnit(BattleUnit unit)
    {
        return (unit.transform.position + (Vector3.up * 1.5f));
    }

    protected void CreateMissText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "MISS", Color.red, 6f, 1f);
    }

    protected void CreateGuardText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "GUARD", Color.gray, 6f, 1f);
    }

    protected void CreateCritText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "CRITICAL!", Color.blue, 9f, 1f);
    }

    protected void CreateKOdText(BattleUnit target)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), "KO'd!", Color.red, 9f, 1f);
    }

    protected void CreateTotalDamageText(BattleUnit target, int damage)
    {
        FloatingTextPopup.Create(GetPositionAboveUnit(target), $"{damage} TOTAL", Color.yellow, 9f, 1f);
    }

    protected void CreateDamageText(BattleUnit target, int damage, bool isCrit)
    {
        DamagePopup.Create(GetPositionAboveUnit(target), damage, isCrit, 1f);
    }

    protected void CreateMeter(BattleUnit target, float startRatio, float endRatio, bool isMP)
    {
        MeterPopup.Create(GetPositionAboveUnit(target), startRatio, endRatio, isMP, 0.25f);
    }

    protected void CreateMeleeHitParticle(BattleUnit target)
    {
        Instantiate(ParticleMaker.GetParticle("MeleeHitParticle"), target.transform.position, Quaternion.identity);
    }

    protected IEnumerator TimedAnnouncement(string message)
    {
        yield return StartCoroutine(TextPopups.AnnounceForSeconds(message, 1f));
    }

    protected IEnumerator AttackCamera(BattleUnit unit, float time = 0f)
    {
        CameraSwitcher.ActionCamera(battleSystem.GetSlotCodeFromUnit(unit));
        yield return new WaitForSeconds(time);
    }

    protected void ApplyDamageMods(ref int damage, bool isCrit, BattleUnit source, BattleUnit target)
    {
        if (isCrit)
        {
            damage *= 2;
            CreateCritText(target);
        }

        if (target.IsGuarding)
        {
            damage /= 2;
            CreateGuardText(target);
        }
    }

    protected void DoSingleHitDamage(int damage, bool crit, BattleUnit target)
    {
        float beforeHPRatio = GetCurrentHPRatio(target);
        target.DamageUnit(damage);
        float afterHPRatio = GetCurrentHPRatio(target);
        CreateMeter(target, beforeHPRatio, afterHPRatio, false);
        CreateMeleeHitParticle(target);
        CreateDamageText(target, damage, crit);
        if (target.IsDead())
        {
            CreateKOdText(target);
        }
    }

    protected void DoAccumulatedDamage(int damage, ref int total, bool crit, BattleUnit target)
    {
        target.DamageUnit(damage);
        total += damage;
        CreateMeleeHitParticle(target);
        CreateDamageText(target, damage, crit);
        if (target.IsDead())
        {
            CreateKOdText(target);
        }
    }
}
