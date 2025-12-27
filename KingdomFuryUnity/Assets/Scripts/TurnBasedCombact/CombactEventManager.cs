using System;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Timeline;

public class CombactEventManager : MonoBehaviour
{
    public CombactUnit target;
    public int amount;
    
    public static CombactEventManager istance = null;

    private void Start()
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

    public void Heal()
    {
        print("healed for");
    }

    public void Damage()
    { 
        print("hit:" + amount.ToString());
        target.HitAnimation();
    }
}
