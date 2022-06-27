using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingMode { SELF, SINGLE_TEAMMATE, ALL_TEAMMATE, SINGLE_ENEMY, ALL_ENEMY, ALL_UNITS, NONE }
public enum UnitSlotCode { P1, P2, P3, P4, E1, E2, E3, E4, E5, NONE }

public class TargetSystem : MonoBehaviour
{
    private BattleMenu battleMenu;
    private BattleSystem battleSystem;

    private UnitSlotCode[] validTargets = null;

    void Awake()
    {
        battleMenu = this.gameObject.GetComponent<BattleMenu>();
        battleSystem = this.gameObject.GetComponent<BattleSystem>();
    }

    void Update()
    {
        if (IsTargetModeActive())
        {
            // Take a look at BattleSystem's selected skill for its targeting mode, current phase (player or enemy), and current turn
            // Interpret those factors into an array of selectable targets
            if (validTargets == null)
            {
                validTargets = GetValidTargets(GetTargetModeFromCurrentAction(), battleSystem.GetCurrentBattleState(), battleSystem.GetCurrentTurnIndex());
            }
        }
        else
        {
            validTargets = null;
        }
    }

    private bool IsTargetModeActive()
    {
        return battleMenu.GetCurrentMenuState() == MenuState.TARGET_MODE;
    }

    private TargetingMode GetTargetModeFromCurrentAction()
    {
        return battleSystem.GetCurrentAction().actionParameters.TargetMode;
    }

    private UnitSlotCode[] GetValidTargets(TargetingMode mode, BattleState state, int index)
    {
        if (state != BattleState.PLAYER && state != BattleState.ENEMY) { return null; } // Narrow state down to two possible states

        UnitSlotCode[] retArray;

        switch (mode)
        {
            case TargetingMode.SELF:
                retArray = (state == BattleState.PLAYER ? new UnitSlotCode[] { GetSlotCode(false, index) } : new UnitSlotCode[] { GetSlotCode(true, index) });
                return retArray;
            case TargetingMode.ALL_UNITS:
                List<UnitSlotCode> tempList = new List<UnitSlotCode>();
                tempList.AddRange(UnitSlotCode.GetValues(typeof(UnitSlotCode))); /* TODO */
                tempList.Remove(UnitSlotCode.NONE);
                retArray = tempList.ToArray();
                return retArray;
            default:
                return null;
        }
    }

    // Helper function for finding the slot code given if it's an enemy and its slot index
    private UnitSlotCode GetSlotCode(bool isEnemy, int index)
    {
        UnitSlotCode retVal;
        return (UnitSlotCode.TryParse<UnitSlotCode>($"{(isEnemy ? "E" : "P")}{index + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
    }
}
