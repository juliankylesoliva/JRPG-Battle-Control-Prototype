using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{
    void Update()
    {
        if (this.transform.childCount < 1)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
