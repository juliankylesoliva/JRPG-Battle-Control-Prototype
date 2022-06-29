using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : ActionScript
{
    public override IEnumerator DoAction()
    {
        sourceUnits[0].IsGuarding = true;
        yield return StartCoroutine(TimedAnnouncement($"{sourceUnits[0].CharacterName} is guarding."));
    }
}
