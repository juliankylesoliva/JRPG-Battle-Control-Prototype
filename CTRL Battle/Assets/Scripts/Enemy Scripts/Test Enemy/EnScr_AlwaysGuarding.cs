using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnScr_AlwaysGuarding : EnemyScript
{
    public override void ActivateEnemy()
    {
        ActionScript chosenAction = ActionMasterList.GetActionScriptByName("Guard");
        battleSystem.SelectAction(chosenAction);
        battleSystem.StartAction(new BattleUnit[] { selfUnit });
    }
}
