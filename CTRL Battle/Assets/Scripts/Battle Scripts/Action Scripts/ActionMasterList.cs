using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMasterList : MonoBehaviour
{
    [SerializeField] GameObject[] normalActionPrefabs;
    [SerializeField] ActionParams[] genericAttackActionParameters;
    [SerializeField] GameObject[] statusObjectPrefabs;

    private static Dictionary<string, ActionScript> actionsList = null;
    private static Dictionary<string, ActionParams> genAtkActParamsList = null;
    private static Dictionary<string, GameObject> statusObjectList = null;

    void Awake()
    {
        InitializeActionsList();
        InitializeGenericAttackActionParametersList();
        InitializeStatusObjectList();
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

    private void InitializeGenericAttackActionParametersList()
    {
        if (genAtkActParamsList != null) { return; }

        genAtkActParamsList = new Dictionary<string, ActionParams>();
        foreach (ActionParams actPar in genericAttackActionParameters)
        {
            genAtkActParamsList.Add(actPar.ActionName, actPar);
        }
    }

    private void InitializeStatusObjectList()
    {
        if (statusObjectList != null) { return; }

        statusObjectList = new Dictionary<string, GameObject>();
        foreach (GameObject obj in statusObjectPrefabs)
        {
            statusObjectList.Add(obj.name, obj);
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

    public static ActionScript GetGenericAttackWithParameterName(string parameterName)
    {
        ActionScript genAct = GetActionScriptByName("GenericAttackAction");
        if (genAct != null && genAtkActParamsList.ContainsKey(parameterName))
        {
            genAct.actionParameters = genAtkActParamsList[parameterName];
            return genAct;
        }
        else
        {
            return null;
        }
    }

    public static ActionParams GetParameterObjectByName(string parameterName)
    {
        if (genAtkActParamsList.ContainsKey(parameterName))
        {
            return genAtkActParamsList[parameterName];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetStatusPrefabByName(string statusName)
    {
        if (statusObjectList.ContainsKey(statusName))
        {
            return statusObjectList[statusName];
        }
        else
        {
            return null;
        }
    }
}
