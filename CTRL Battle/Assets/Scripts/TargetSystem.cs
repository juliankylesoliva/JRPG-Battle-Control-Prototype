using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingMode { SELF, SINGLE_TEAMMATE, ALL_TEAMMATE, SINGLE_ENEMY, ALL_ENEMY, ALL_UNITS, NONE }
public enum UnitTargetStatus { ALIVE, DEAD, EITHER, NONE }
public enum UnitSlotCode { P1, P2, P3, P4, E1, E2, E3, E4, E5, NONE }

public class TargetSystem : MonoBehaviour
{
    private BattleMenu battleMenu;
    private BattleSystem battleSystem;

    private TargetingMode currentMode = TargetingMode.NONE;
    private UnitTargetStatus targetStatus = UnitTargetStatus.NONE;
    private UnitSlotCode[] possibleTargetCodes = null;
    private BattleUnit[] validUnits = null;

    void Awake()
    {
        battleMenu = this.gameObject.GetComponent<BattleMenu>();
        battleSystem = this.gameObject.GetComponent<BattleSystem>();
    }

    void Update()
    {
        if (IsTargetModeActive())
        {
            // Take a look at BattleSystem's selected skill for its targeting mode, current phase (player or enemy), required unit status, and current turn
            // Interpret those factors into an array of selectable targets
            if (validUnits == null && possibleTargetCodes == null && targetStatus == UnitTargetStatus.NONE && currentMode == TargetingMode.NONE)
            {
                currentMode = GetTargetModeFromCurrentAction();
                targetStatus = GetTargetStatusFromCurrentAction();
                possibleTargetCodes = GetPossibleTargets(currentMode, battleSystem.GetCurrentBattleState(), battleSystem.GetCurrentTurnIndex());
                validUnits = GetValidUnits(targetStatus, possibleTargetCodes);
                FocusCameraOnTargets();
                AnnounceTarget();
            }

            // If the player clicks on a valid unit, confirm the action with the battle system by sending the target(s).
            CheckMouseTargetSelect();
        }
        else
        {
            currentMode = TargetingMode.NONE;
            targetStatus = UnitTargetStatus.NONE;
            possibleTargetCodes = null;
            validUnits = null;
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

    private UnitTargetStatus GetTargetStatusFromCurrentAction()
    {
        return battleSystem.GetCurrentAction().actionParameters.TargetStatus;
    }

    // Helper function for switching the camera to the targets
    private void FocusCameraOnTargets()
    {
        switch (currentMode)
        {
            case TargetingMode.SELF:
                break;
            case TargetingMode.SINGLE_TEAMMATE:
            case TargetingMode.ALL_TEAMMATE:
                CameraSwitcher.ChangeToCamera("PlayerTargetCam");
                break;
            case TargetingMode.SINGLE_ENEMY:
            case TargetingMode.ALL_ENEMY:
                CameraSwitcher.ChangeToCamera("EnemyTargetCam");
                break;
            case TargetingMode.ALL_UNITS:
                CameraSwitcher.ChangeToCamera("AllTargetCam");
                break;
            default:
                CameraSwitcher.ChangeToCamera("OverviewCam");
                break;
        }
    }

    // Helper function for announcing the targeting mode
    private void AnnounceTarget()
    {
        switch (currentMode)
        {
            case TargetingMode.SELF:
                TextPopups.Announce("Targeting yourself!");
                break;
            case TargetingMode.SINGLE_TEAMMATE:
                TextPopups.Announce("Select a teammate!");
                break;
            case TargetingMode.ALL_TEAMMATE:
                TextPopups.Announce("Targeting the whole party!");
                break;
            case TargetingMode.SINGLE_ENEMY:
                TextPopups.Announce("Select an enemy!");
                break;
            case TargetingMode.ALL_ENEMY:
                TextPopups.Announce("Targeting all enemies!");
                break;
            case TargetingMode.ALL_UNITS:
                TextPopups.Announce("Targeting everyone!");
                break;
            default:
                Debug.Log("No targets?");
                break;
        }
    }

    // Helper function for selecting a valid target with the mouse
    private void CheckMouseTargetSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 999f))
            {
                foreach (BattleUnit b in validUnits)
                {
                    if (GameObject.ReferenceEquals(b.gameObject, hit.transform.parent.gameObject)) // The hit was on the model, not the actual gameobject
                    {
                        switch (currentMode)
                        {
                            case TargetingMode.ALL_TEAMMATE:
                            case TargetingMode.ALL_ENEMY:
                            case TargetingMode.ALL_UNITS:
                                battleSystem.StartAction(validUnits);
                                break;
                            case TargetingMode.SELF:
                            case TargetingMode.SINGLE_TEAMMATE:
                            case TargetingMode.SINGLE_ENEMY:
                                battleSystem.StartAction(new BattleUnit[] {b});
                                break;
                            case TargetingMode.NONE:
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    private BattleUnit[] GetValidUnits(UnitTargetStatus status, UnitSlotCode[] codes)
    {
        List<BattleUnit> tempList = new List<BattleUnit>();
        foreach (UnitSlotCode usc in codes)
        {
            BattleUnit toAdd = battleSystem.GetUnitFromSlotCode(usc);
            if (toAdd != null)
            {
                switch (status)
                {
                    case UnitTargetStatus.ALIVE:
                        if (!toAdd.IsDead()) { tempList.Add(toAdd); }
                        break;
                    case UnitTargetStatus.DEAD:
                        if (toAdd.IsDead()) { tempList.Add(toAdd); }
                        break;
                    case UnitTargetStatus.EITHER:
                    default:
                        tempList.Add(toAdd);
                        break;
                }
            }
        }
        return tempList.ToArray();
    }

    private UnitSlotCode[] GetPossibleTargets(TargetingMode mode, BattleState state, int index)
    {
        if (state != BattleState.PLAYER && state != BattleState.ENEMY) { return null; } // Narrow state down to two possible states

        switch (mode)
        {
            case TargetingMode.SELF:
                return GetSelfCode(mode, state, index);
            case TargetingMode.ALL_UNITS:
                return GetAllSlotCodes(mode, state, index);
            case TargetingMode.SINGLE_TEAMMATE:
            case TargetingMode.ALL_TEAMMATE:
                return GetAllTeammates(mode, state, index);
            case TargetingMode.SINGLE_ENEMY:
            case TargetingMode.ALL_ENEMY:
                return GetAllEnemies(mode, state, index);
            case TargetingMode.NONE:
            default:
                return null;
        }
    }

    // Helper functions for each case in GetPossibleTargets() function
    private UnitSlotCode[] GetSelfCode(TargetingMode mode, BattleState state, int index)
    {
        return (state == BattleState.PLAYER ? new UnitSlotCode[] { GetSlotCode(false, index) } : new UnitSlotCode[] { GetSlotCode(true, index) });
    }

    private UnitSlotCode[] GetAllSlotCodes(TargetingMode mode, BattleState state, int index)
    {
        List<UnitSlotCode> tempList = new List<UnitSlotCode>();
        UnitSlotCode[] tempArray = (UnitSlotCode[])UnitSlotCode.GetValues(typeof(UnitSlotCode));
        foreach (UnitSlotCode usc in tempArray)
        {
            if (usc != UnitSlotCode.NONE)
            {
                tempList.Add(usc);
            }
        }
        return tempList.ToArray();
    }

    private UnitSlotCode[] GetAllTeammates(TargetingMode mode, BattleState state, int index)
    {
        List<UnitSlotCode> tempList = new List<UnitSlotCode>();
        UnitSlotCode[] tempArray = (UnitSlotCode[])UnitSlotCode.GetValues(typeof(UnitSlotCode));
        if (state == BattleState.PLAYER)
        {
            foreach (UnitSlotCode usc in tempArray)
            {
                if (usc.ToString()[0] == 'P')
                {
                    tempList.Add(usc);
                }
            }
        }
        else
        {
            foreach (UnitSlotCode usc in tempArray)
            {
                if (usc.ToString()[0] == 'E')
                {
                    tempList.Add(usc);
                }
            }
        }
        return tempList.ToArray();
    }

    private UnitSlotCode[] GetAllEnemies(TargetingMode mode, BattleState state, int index)
    {
        List<UnitSlotCode> tempList = new List<UnitSlotCode>();
        UnitSlotCode[] tempArray = (UnitSlotCode[])UnitSlotCode.GetValues(typeof(UnitSlotCode));
        if (state == BattleState.PLAYER)
        {
            foreach (UnitSlotCode usc in tempArray)
            {
                if (usc.ToString()[0] == 'E')
                {
                    tempList.Add(usc);
                }
            }
        }
        else
        {
            foreach (UnitSlotCode usc in tempArray)
            {
                if (usc.ToString()[0] == 'P')
                {
                    tempList.Add(usc);
                }
            }
        }
        return tempList.ToArray();
    }

    // Helper function for finding the slot code given if it's an enemy and its slot index
    private UnitSlotCode GetSlotCode(bool isEnemy, int index)
    {
        UnitSlotCode retVal;
        return (UnitSlotCode.TryParse<UnitSlotCode>($"{(isEnemy ? "E" : "P")}{index + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
    }
}
