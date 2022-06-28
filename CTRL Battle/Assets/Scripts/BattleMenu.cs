using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState { STANDBY, HOME, TARGET_MODE, GUARD_CONFIRMATION, CTRL_MENU, SKILL_MENU, ITEMS_MENU, TACTICS_MENU}

public class BattleMenu : MonoBehaviour
{
    private BattleSystem battleSystem;

    private MenuState currentMenuState = MenuState.STANDBY;
    private List<MenuState> previousMenuStates;

    void Awake()
    {
        battleSystem = this.gameObject.GetComponent<BattleSystem>();
    }
    
    void Start()
    {
        previousMenuStates = new List<MenuState>();
    }

    void Update()
    {
        CheckInputs();
    }

    // Helper function for checking inputs to change state
    private void CheckInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (currentMenuState == MenuState.HOME) // Initiate Guard
            {
                battleSystem.SelectAction(ActionMasterList.GetActionScriptByName("Guard"), new BattleUnit[] {battleSystem.GetCurrentUnit()});
                GoToNextMenu(MenuState.GUARD_CONFIRMATION);
            }
            else // Go back
            {
                battleSystem.DeselectAction();
                GoToPreviousMenu();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentMenuState == MenuState.HOME) // Initiate Melee attack
            {
                // NOTE: Melee action script used here may depend on a character's weapon in the future.
                battleSystem.SelectAction(ActionMasterList.GetActionScriptByName("MeleeAttack"), new BattleUnit[] {battleSystem.GetCurrentUnit()});
                GoToNextMenu(MenuState.TARGET_MODE);
            }
            else if (currentMenuState == MenuState.GUARD_CONFIRMATION) // Confirm Guard
            {
                battleSystem.StartAction();
            }
            else {/* Nothing */}
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentMenuState == MenuState.HOME) // Activate projectile mode
            {
                if (UnitHasAmmo())
                {
                    battleSystem.SelectAction(ActionMasterList.GetActionScriptByName("ProjectileAttack"), new BattleUnit[] { battleSystem.GetCurrentUnit() });
                    GoToNextMenu(MenuState.TARGET_MODE);
                }
                else
                {
                    Debug.Log("Out of ammo!");
                }
            }
            // NOTE: Add reload function if player presses Tab again?
        }
        else {/* Nothing */}
    }

    // Helper function for going to the next state in the battle menu
    private void GoToNextMenu(MenuState nextState)
    {
        previousMenuStates.Add(currentMenuState);
        currentMenuState = nextState;
        Debug.Log($"Went to {currentMenuState}");
    }

    // Helper function for going back in the battle menu
    private void GoToPreviousMenu()
    {
        if (currentMenuState != MenuState.STANDBY && previousMenuStates.Count > 0)
        {
            int lastIndex = (previousMenuStates.Count - 1);
            currentMenuState = previousMenuStates[lastIndex];
            previousMenuStates.RemoveAt(lastIndex);
            Debug.Log($"Went back to {currentMenuState}");
        }
    }

    // OnClick Button function for setting target mode
    public void SetToTargetMode()
    {
        GoToNextMenu(MenuState.TARGET_MODE);
    }

    // Helper function for clearing the previous state stack and setting the standby state
    public void ClearStackStandby()
    {
        previousMenuStates.Clear();
        currentMenuState = MenuState.STANDBY;
    }

    // Helper function for switching from standby state to home state
    public void StandbyToHome()
    {
        if (currentMenuState == MenuState.STANDBY) { currentMenuState = MenuState.HOME; }
    }

    public MenuState GetCurrentMenuState()
    {
        return currentMenuState;
    }

    // Helper function for checking a unit's ammo count
    private bool UnitHasAmmo()
    {
        return battleSystem.GetCurrentUnit().AmmoLoaded > 0;
    }
}
