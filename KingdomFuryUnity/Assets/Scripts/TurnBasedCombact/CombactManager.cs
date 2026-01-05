using System.Collections;
using System.Collections.Generic;
using TurnBasedCombact.Model;
using UnityEngine;

public class CombactManager : MonoBehaviour
{

    public CombactUnit combactUnit;
    public CombactUnit enemy;
    public CombactUnit enemy2;

    public static CombactManager istance = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (istance == null)
        {
            istance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartAction()
    {
        CombactInfo combactInfo = new CombactInfo();
        combactInfo.enemyTargets = new List<CombactUnit>();
        combactInfo.enemyTargets.Add(enemy);
        combactInfo.enemyTargets.Add(enemy2);
        
        combactUnit.ExecuteAction(0,  combactInfo);
    }

}
