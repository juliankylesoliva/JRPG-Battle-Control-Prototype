using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyScript : MonoBehaviour
{
    protected BattleSystem battleSystem;
    protected BattleUnit selfUnit;

    void Awake()
    {
        battleSystem = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        selfUnit = this.gameObject.GetComponent<BattleUnit>();
    }

    public abstract void ActivateEnemy();
}
