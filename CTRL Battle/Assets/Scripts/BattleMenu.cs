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
                battleSystem.SelectAction(ActionMasterList.GetActionScriptByName("Guard"));
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
                battleSystem.SelectAction(ActionMasterList.GetActionScriptByName(battleSystem.GetCurrentUnit().Melee));
                GoToNextMenu(MenuState.TARGET_MODE);
            }
            else if (currentMenuState == MenuState.GUARD_CONFIRMATION) // Confirm Guard
            {
                battleSystem.StartAction(new BattleUnit[] { battleSystem.GetCurrentUnit() });
            }
            else {/* Nothing */}
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (currentMenuState == MenuState.HOME) // Activate projectile attack
            {
                if (UnitHasAmmo())
                {
                    battleSystem.SelectAction(ActionMasterList.GetActionScriptByName(battleSystem.GetCurrentUnit().Projectile));
                    GoToNextMenu(MenuState.TARGET_MODE);
                }
                else
                {
                    Debug.Log("Out of ammo!");
                }
            }
        }
        else {/* Nothing */}
    }

    // Helper function for doing certain actions when switcing to a specific menu state.
    private void CheckMenuState()
    {
        switch (currentMenuState)
        {
            case MenuState.HOME:
                TextPopups.Announce($"It's {battleSystem.GetCurrentUnit().CharacterName}'s turn...");
                CameraSwitcher.ChangeToCamera($"{battleSystem.GetCurrentUnitSlotCode()}Cam");
                break;
            case MenuState.GUARD_CONFIRMATION:
                TextPopups.Announce("Guard until your next turn?");
                break;
            case MenuState.TARGET_MODE:
                break;
            default:
                TextPopups.Announce("");
                break;
        }
    }

    // Helper function for going to the next state in the battle menu
    private void GoToNextMenu(MenuState nextState)
    {
        previousMenuStates.Add(currentMenuState);
        currentMenuState = nextState;
        CheckMenuState();
    }

    // Helper function for going back in the battle menu
    private void GoToPreviousMenu()
    {
        if (currentMenuState != MenuState.STANDBY && previousMenuStates.Count > 0)
        {
            int lastIndex = (previousMenuStates.Count - 1);
            currentMenuState = previousMenuStates[lastIndex];
            previousMenuStates.RemoveAt(lastIndex);
            CheckMenuState();
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
        CheckMenuState();
    }

    // Helper function for switching from standby state to home state
    public void StandbyToHome()
    {
        if (currentMenuState == MenuState.STANDBY)
        {
            currentMenuState = MenuState.HOME;
            CheckMenuState();
        }
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
