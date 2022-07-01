using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool reverseDirection = false;

    void Update()
    {
        this.transform.forward = Camera.main.transform.forward * (reverseDirection ? -1f : 1f);
    }
}
