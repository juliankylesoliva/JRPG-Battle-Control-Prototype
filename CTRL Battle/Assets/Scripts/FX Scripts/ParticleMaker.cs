using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMaker : MonoBehaviour
{
    [SerializeField] GameObject[] particleList;

    private static Dictionary<string, GameObject> particleDict = null;

    void Awake()
    {
        InitializeParticleDictionary();
    }

    private void InitializeParticleDictionary()
    {
        if (particleDict == null)
        {
            particleDict = new Dictionary<string, GameObject>();
            foreach (GameObject p in particleList)
            {
                particleDict.Add(p.name, p);
            }
        }
    }

    public static GameObject GetParticle(string name)
    {
        if (particleDict.ContainsKey(name))
        {
            return particleDict[name];
        }
        return null;
    }
}
