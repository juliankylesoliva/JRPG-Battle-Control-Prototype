using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMasterList : MonoBehaviour
{
    [SerializeField] ActionScript[] normalActionPrefabs;

    private static Dictionary<string, ActionScript> actionsList = null;

    void Awake()
    {
        InitializeActionsList();
    }

    private void InitializeActionsList()
    {
        actionsList = new Dictionary<string, ActionScript>();
        foreach (ActionScript action in normalActionPrefabs)
        {
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
