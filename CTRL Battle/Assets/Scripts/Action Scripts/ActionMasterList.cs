using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMasterList : MonoBehaviour
{
    [SerializeField] GameObject[] normalActionPrefabs;

    private static Dictionary<string, ActionScript> actionsList = null;

    void Awake()
    {
        InitializeActionsList();
    }

    private void InitializeActionsList()
    {
        if (actionsList != null) { return; }

        actionsList = new Dictionary<string, ActionScript>();
        foreach (GameObject actionObj in normalActionPrefabs)
        {
            ActionScript action = Instantiate(actionObj, this.transform).GetComponent<ActionScript>();
            actionsList.Add(action.GetType().Name, action);
        }
    }

    public static ActionScript GetActionScriptByName(string actionName)
    {
        if (actionsList.ContainsKey(actionName))
        {
            return actionsList[actionName];
        }
        else
        {
            return null;
        }
    }
}
