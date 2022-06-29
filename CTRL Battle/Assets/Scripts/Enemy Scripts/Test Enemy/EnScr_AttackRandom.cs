using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnScr_AttackRandom : EnemyScript
{
    public override void ActivateEnemy()
    {
        ActionScript chosenAction = ActionMasterList.GetActionScriptByName("MeleeAttack");
        battleSystem.SelectAction(chosenAction, new BattleUnit[] {selfUnit});

        BattleUnit chosenTarget;
        UnitSlotCode[] possibleTargets;

        List<UnitSlotCode> tempList = new List<UnitSlotCode>();
        UnitSlotCode[] tempArray = (UnitSlotCode[])UnitSlotCode.GetValues(typeof(UnitSlotCode));
        foreach (UnitSlotCode usc in tempArray)
        {
            if (usc.ToString()[0] == 'P')
            {
                tempList.Add(usc);
            }
        }
        possibleTargets = tempList.ToArray();

        do
        {
            chosenTarget = battleSystem.GetUnitFromSlotCode(possibleTargets[Random.Range(0, possibleTargets.Length)]);
        } while (chosenTarget == null);

        battleSystem.StartAction(new BattleUnit[] {chosenTarget});
    }
}
