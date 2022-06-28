using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : ActionScript
{
    public override IEnumerator DoAction()
    {
        sourceUnits[0].IsGuarding = true;
        Debug.Log($"{sourceUnits[0].CharacterName} is guarding.");
        yield return new WaitForSeconds(1f);
    }
}
