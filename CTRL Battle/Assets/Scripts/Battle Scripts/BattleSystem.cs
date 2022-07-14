using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYER, ENEMY, VICTORY, DEFEAT }
public enum EncounterType { NORMAL, PLAYER_AMBUSH, ENEMY_AMBUSH }

public class BattleSystem : MonoBehaviour
{
    // The battle menu component
    private BattleMenu battleMenu;

    // Prefab and transform for the character status panels
    [SerializeField] GameObject statusPanelPrefab;
    [SerializeField] Transform statusPanelTranform;

    // Encounter type determines how the first team is chosen
    [SerializeField] EncounterType encounterType = EncounterType.NORMAL;

    // Prefab objects to spawn into the battle
    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] GameObject[] playerCTRLPrefabs;
    [SerializeField] GameObject[] enemyPrefabs;

    // Transform objects to parent players and enemies to
    [SerializeField] Transform[] playerSlots;
    [SerializeField] Transform[] enemySlots;

    // Arrays of indices from the respective prefab list to spawn in
    [SerializeField] int[] playerIndices;
    [SerializeField] int[] enemyIndices;

    // Arrays of BattleUnit data for access by relevant battle management classes
    private BattleUnit[] playerUnits = new BattleUnit[4];
    private BattleUnit[] enemyUnits = new BattleUnit[5];

    // The current battle state
    private BattleState currentState = BattleState.START;

    // The current player/enemy slot turn
    private int currentTurn = -1;

    // The current action being taken
    private ActionScript currentAction = null;

    void Awake()
    {
        battleMenu = this.gameObject.GetComponent<BattleMenu>();
    }

    void Start()
    {
        StartCoroutine(BattleSetup());
    }

    // Helper functions that set up the player and enemy characters
    private IEnumerator BattleSetup()
    {
        currentState = BattleState.START;
        SetupPlayers();
        SetupEnemies();
        yield return StartCoroutine(BattleStartMessage());
        StartCoroutine(ChangePhase());
    }

    private IEnumerator BattleStartMessage()
    {
        switch (encounterType)
        {
            case EncounterType.NORMAL:
                yield return StartCoroutine(TextPopups.AnnounceForSeconds("You encountered the enemy!", 2f));
                break;
            case EncounterType.PLAYER_AMBUSH:
                yield return StartCoroutine(TextPopups.AnnounceForSeconds("You ambushed the enemy!!!", 2f));
                break;
            case EncounterType.ENEMY_AMBUSH:
                yield return StartCoroutine(TextPopups.AnnounceForSeconds("You have been ambushed!!!", 2f));
                break;
        }
    }

    private void SetupPlayers()
    {
        for (int i = 0; i < playerIndices.Length && i < playerSlots.Length; ++i) // Boolean condition forces party limit of four while accounting for smaller party lineups
        {
            int chosenIndex = playerIndices[i]; // Get index from player indices array
            GameObject tempObj = Instantiate(playerPrefabs[chosenIndex], playerSlots[i]); // Spawn the chosen player prefab to the correct player slot
            playerUnits[i] = tempObj.GetComponent<BattleUnit>(); // Save the BattleUnit component from the spawned-in prefab
        }

        for (int i = playerUnits.Length - 1; i >= 0; --i)
        {
            BattleUnit unit = playerUnits[i];
            if (unit != null)
            {
                GameObject tempObj = Instantiate(statusPanelPrefab, statusPanelTranform);
                StatusPanel tempPanel = tempObj.GetComponent<StatusPanel>();
                tempPanel.SetBattleUnit(unit);
            }
        }
    }

    private void SetupEnemies()
    {
        // Similar comments to SetupPlayers() function
        for (int i = 0; i < enemyIndices.Length && i < enemySlots.Length; ++i)
        {
            int chosenIndex = enemyIndices[i];
            GameObject tempObj = Instantiate(enemyPrefabs[chosenIndex], enemySlots[i]);
            enemyUnits[i] = tempObj.GetComponent<BattleUnit>();
        }
    }

    // Helper function for switching between Player and Enemy Phases
    private IEnumerator ChangePhase()
    {
        // Reset current turn index
        currentTurn = -1;

        // Switch the current battle state
        switch (currentState)
        {
            case BattleState.START: // Start ---> Player or Enemy*
                switch (encounterType)
                {
                    case EncounterType.NORMAL:
                        currentState = GetFastestTeam();
                        break;
                    case EncounterType.PLAYER_AMBUSH:
                        currentState = BattleState.PLAYER;
                        break;
                    case EncounterType.ENEMY_AMBUSH:
                        currentState = BattleState.ENEMY;
                        break;
                }
                break;
            case BattleState.PLAYER: // Player ---> Enemy
                currentState = BattleState.ENEMY;
                break;
            case BattleState.ENEMY: // Enemy ---> Player
                currentState = BattleState.PLAYER;
                break;
            default:
                /* N/A */
                break;
        }

        // Announce phase change
        if (currentState == BattleState.PLAYER)
        {
            CameraSwitcher.ChangeToCamera("PlayerPhaseCam");
            yield return StartCoroutine(TextPopups.AnnounceForSeconds("PLAYER PHASE", 1.5f));
        }
        else if (currentState == BattleState.ENEMY)
        {
            CameraSwitcher.ChangeToCamera("EnemyPhaseCam");
            yield return StartCoroutine(TextPopups.AnnounceForSeconds("ENEMY PHASE", 1.5f));
        }
        else { yield return null; }

        // Start the first available team member's turn
        if (currentState == BattleState.PLAYER) { GoToNextPlayerPartyMember(); }
        else if (currentState == BattleState.ENEMY) { GoToNextEnemyPartyMember(); }
        else { /* N/A */ }
    }

    // Helper function for determining which team goes first
    private BattleState GetFastestTeam()
    {
        // Get the total speed for the player's party
        int playerTeamSpeed = 0;
        foreach (BattleUnit unit in playerUnits)
        {
            if (unit != null)
            {
                playerTeamSpeed += unit.Agility;
            }
        }

        // Get the total speed for the enemy's party
        int enemyTeamSpeed = 0;
        foreach (BattleUnit unit in enemyUnits)
        {
            if (unit != null)
            {
                enemyTeamSpeed += unit.Agility;
            }
        }

        // Compare total speeds
        if (playerTeamSpeed > enemyTeamSpeed) // Player's party is faster
        {
            return BattleState.PLAYER;
        }
        else if (enemyTeamSpeed > playerTeamSpeed) // Enemy's party is faster
        {
            return BattleState.ENEMY;
        }
        else // Ties are broken by coin flip (50/50)
        {
            return (Random.Range(0f, 1f) <= 0.5f ? BattleState.PLAYER : BattleState.ENEMY);
        }
    }

    // Public helper function for spawning in character controllers for CTRL attacks.
    public GameObject SpawnCTRLCharacter(int index, Vector3 position)
    {
        return Instantiate(playerCTRLPrefabs[index], position, Quaternion.identity);
    }

    // Public helper function to be called by action scripts to notify the system to end the current turn
    public IEnumerator EndOfTurn()
    {
        EnemyCleanup();
        if (!IsBattleOver()) // Check if either side won
        {
            yield return StartCoroutine(GetCurrentUnit().ResolveStatuses(ResolveType.END_OF_TURN));
            EnemyCleanup();

            if (!IsBattleOver())
            {
                switch (currentState)
                {
                    case BattleState.PLAYER:
                        GoToNextPlayerPartyMember();
                        break;
                    case BattleState.ENEMY:
                        GoToNextEnemyPartyMember();
                        break;
                    default:
                        break;
                }
            }
        }
        yield return null;
    }

    // Helper functions for getting the next player/enemy team member's turn
    private void GoToNextPlayerPartyMember()
    {
        do
        {
            currentTurn++;
        } while (currentTurn < playerSlots.Length && (playerUnits[currentTurn] == null || playerUnits[currentTurn].IsDead()));
        
        if (currentTurn < playerSlots.Length)
        {
            BattleUnit currentUnit = GetCurrentUnit();
            currentUnit.IsGuarding = false;
            currentUnit.ReloadAmmo();
            battleMenu.StandbyToHome();
        }
        else
        {
            StartCoroutine(ChangePhase());
        }
    }

    private void GoToNextEnemyPartyMember()
    {
        do
        {
            currentTurn++;
        } while (currentTurn < enemySlots.Length && (enemyUnits[currentTurn] == null || enemyUnits[currentTurn].IsDead()));

        if (currentTurn < enemySlots.Length)
        {
            BattleUnit currentUnit = GetCurrentUnit();
            currentUnit.IsGuarding = false;
            CameraSwitcher.ChangeToCamera($"{GetCurrentUnitSlotCode()}Cam");
            ActivateEnemyAI(currentUnit.gameObject);
        }
        else
        {
            StartCoroutine(ChangePhase());
        }
    }

    // Helper functions for checking the win/lose conditions before a turn change
    private bool IsBattleOver()
    {
        if (!IsPlayerPartyDefeated() && !IsEnemyPartyDefeated()) { return false; }
        else
        {
            CameraSwitcher.ChangeToCamera("OverviewCamera");
            if (IsPlayerPartyDefeated()) { TextPopups.Announce("You lost the battle..."); }
            else if (IsEnemyPartyDefeated()) { TextPopups.Announce("YOU WON!"); }
            else { /* N/A */ }

            return true;
        }
    }

    private bool IsPlayerPartyDefeated()
    {
        foreach (BattleUnit unit in playerUnits)
        {
            if (unit != null && !unit.IsDead())
            {
                return false;
            }
        }
        return true;
    }

    private bool IsEnemyPartyDefeated()
    {
        foreach (BattleUnit unit in enemyUnits)
        {
            if (unit != null && !unit.IsDead())
            {
                return false;
            }
        }
        return true;
    }

    // Helper function for deleting defeated enemies
    private void EnemyCleanup()
    {
        for (int i = 0; i < enemyUnits.Length; ++i)
        {
            BattleUnit unit = enemyUnits[i];
            if (unit != null && unit.IsDead())
            {
                GameObject.Destroy(unit.gameObject);
                enemyUnits[i] = null;
            }
        }
    }

    // Public functions for selecting/deselecting/starting/getting the current action
    public void SelectAction(ActionScript action)
    {
        if (currentAction == null)
        {
            currentAction = action;
        }
    }

    public void DeselectAction()
    {
        if (currentAction != null)
        {
            currentAction.SetSourceUnits(null);
            currentAction.SetTargetUnits(null);
            currentAction = null;
        }
    }

    public void StartAction(BattleUnit[] sources = null, BattleUnit[] targets = null)
    {
        if (currentAction != null)
        {
            if (sources != null)
            {
                currentAction.SetSourceUnits(sources);
            }

            if (targets != null)
            {
                currentAction.SetTargetUnits(targets);
            }

            currentAction.StartAction();
            battleMenu.ClearStackStandby();
            currentAction = null;
        }
    }

    public ActionScript GetCurrentAction()
    {
        return currentAction;
    }

    // Helper function for activating the enemy's attached AI script.
    private void ActivateEnemyAI(GameObject enemyObj)
    {
        EnemyScript enemyAI = enemyObj.GetComponent<EnemyScript>();
        if (enemyAI != null)
        {
            enemyAI.ActivateEnemy();
        }
    }

    // Public function for returning the current battle state
    public BattleState GetCurrentBattleState()
    {
        return currentState;
    }

    // Public function for returning the current turn index
    public int GetCurrentTurnIndex()
    {
        return currentTurn;
    }

    // Public function for getting unit data from slot codes
    public BattleUnit GetUnitFromSlotCode(UnitSlotCode usc)
    {
        string code = usc.ToString();
        int index = (int)char.GetNumericValue(code[1]);
        if (code[0] == 'P')
        {
            if (index >= 1 && index <= 4)
            {
                return playerUnits[index - 1];
            }
            return null;
        }
        else if (code[0] == 'E')
        {
            if (index >= 1 && index <= 5)
            {
                return enemyUnits[index - 1];
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    // Public function for getting slot codes from unit data
    public UnitSlotCode GetSlotCodeFromUnit(BattleUnit unit)
    {
        UnitSlotCode retVal;

        for (int i = 0; i < playerUnits.Length; ++i)
        {
            BattleUnit bu = playerUnits[i];
            if (GameObject.ReferenceEquals(bu, unit))
            {
                return (UnitSlotCode.TryParse<UnitSlotCode>($"P{i + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
            }
        }

        for (int i = 0; i < enemyUnits.Length; ++i)
        {
            BattleUnit bu = enemyUnits[i];
            if (GameObject.ReferenceEquals(bu, unit))
            {
                return (UnitSlotCode.TryParse<UnitSlotCode>($"E{i + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
            }
        }

        return UnitSlotCode.NONE;
    }

    // Public function for finding out if a unit is an enemy or not
    public bool IsUnitAnEnemy(BattleUnit unit)
    {
        UnitSlotCode usc = GetSlotCodeFromUnit(unit);
        string code = usc.ToString();
        return code[0] == 'E';
    }

    // Public helper function for getting the current unit's turn
    public BattleUnit GetCurrentUnit()
    {
        if (currentState != BattleState.PLAYER && currentState != BattleState.ENEMY) { return null; }

        if (currentState == BattleState.PLAYER)
        {
            if (currentTurn >= 0 && currentTurn < playerSlots.Length)
            {
                return playerUnits[currentTurn];
            }
            return null;
        }
        else // BattleState.ENEMY
        {
            if (currentTurn >= 0 && currentTurn < enemySlots.Length)
            {
                return enemyUnits[currentTurn];
            }
            return null;
        }
    }

    // Public helper function for getting the unit slot code of the current unit's turn
    public UnitSlotCode GetCurrentUnitSlotCode()
    {
        if (currentState != BattleState.PLAYER && currentState != BattleState.ENEMY) { return UnitSlotCode.NONE; }

        UnitSlotCode retVal;

        if (currentState == BattleState.PLAYER)
        {
            if (currentTurn >= 0 && currentTurn < playerSlots.Length)
            {
                return (UnitSlotCode.TryParse<UnitSlotCode>($"P{currentTurn + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
            }
            return UnitSlotCode.NONE;
        }
        else // BattleState.ENEMY
        {
            if (currentTurn >= 0 && currentTurn < enemySlots.Length)
            {
                return (UnitSlotCode.TryParse<UnitSlotCode>($"E{currentTurn + 1}", out retVal) ? retVal : UnitSlotCode.NONE);
            }
            return UnitSlotCode.NONE;
        }
    }
}
