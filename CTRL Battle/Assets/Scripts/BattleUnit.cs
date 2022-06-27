using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] int CurrentHP = 10;
    [SerializeField] int MaxHP = 10;

    [SerializeField] int ATK = 1;
    [SerializeField] int DEF = 1;
    [SerializeField] int AGI = 1;

    /* GETTER FUNCTIONS */
    public int GetAgility()
    {
        return AGI;
    }

    public bool IsDead()
    {
        return CurrentHP <= 0;
    }
}
