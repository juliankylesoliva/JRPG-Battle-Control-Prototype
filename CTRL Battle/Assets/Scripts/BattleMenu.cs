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
            if (currentMenuState == MenuState.HOME) // Guard
            {
                Debug.Log("[SPACE]: Guard | [LShift]: Cancel");
                GoToNextMenu(MenuState.GUARD_CONFIRMATION);
            }
            else // Go back
            {
                GoToPreviousMenu();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentMenuState == MenuState.HOME) // Melee attack
            {
                battleSystem.SelectAction(ActionMasterList.GetActionScriptByName("MeleeAttack"));
            }
            else if (currentMenuState == MenuState.GUARD_CONFIRMATION) // Confirm Guard
            {

            }
            else {/* Nothing */}
        }
        else {/* Nothing */}
    }

    // Helper function for going to the next state in the battle menu
    private void GoToNextMenu(MenuState nextState)
    {
        previousMenuStates.Add(currentMenuState);
        currentMenuState = nextState;
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
}
