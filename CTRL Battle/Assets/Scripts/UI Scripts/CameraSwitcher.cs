using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] string initialCamera = "OverviewCamera";
    [SerializeField] CinemachineVirtualCamera[] cameraList;

    private static Dictionary<string, CinemachineVirtualCamera> cameraDictionary = null;
    private static int currentPriority = 0;

    void Awake()
    {
        InitializeCameraDictionary();
        ChangeToCamera(initialCamera);
    }

    private void InitializeCameraDictionary()
    {
        if (cameraDictionary != null) { return; }

        cameraDictionary = new Dictionary<string, CinemachineVirtualCamera>();
        foreach (CinemachineVirtualCamera cam in cameraList)
        {
            cam.Priority = currentPriority;
            cameraDictionary.Add(cam.gameObject.name, cam);
        }
    }

    public static void ChangeToCamera(string cameraName)
    {
        if (cameraDictionary.ContainsKey(cameraName))
        {
            currentPriority++;
            cameraDictionary[cameraName].Priority = currentPriority;
        }
    }

    public static void ActionCamera(UnitSlotCode usc)
    {
        string code = usc.ToString();
        if (code[0] == 'P')
        {
            ChangeToCamera($"{usc}CamBehind");
        }
        else if (code[0] == 'E')
        {
            ChangeToCamera($"{usc}Cam");
        }
        else {/* Nothing */}
        
    }
}
