using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Limiter : MonoBehaviour
{
    [SerializeField] int FPS = 60;

    void Awake()
    {
        Application.targetFrameRate = FPS;
    }
}
