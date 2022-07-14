using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCTRLAtk : ActionScript
{
    protected override IEnumerator DoAction()
    {
        BattleUnit user = sourceUnits[0];
        BattleUnit target = targetUnits[0];

        yield return CTRLAttackCamera(0.5f);
        yield return StartCoroutine(TimedAnnouncement($"{user.CharacterName} is in CTRL!"));

        GameObject ctrlObj = battleSystem.SpawnCTRLCharacter(0, actionParameters.PlayerSpawnLocation);

        CTRLMeter.StartTimer(actionParameters.BaseAttackDuration);
        while (CTRLMeter.IsTimerActive())
        {
            yield return null;
        }

        GameObject.Destroy(ctrlObj);
        yield return WaitASec;

        yield return null;
    }
}
