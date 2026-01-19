using System.Collections.Generic;
using Systems.CombactActionSystem;
using UnityEngine;


public class UnitController: MonoBehaviour
{
    List<IAction> actions = new List<IAction>();
    // UnitInfo unitInfo;
    // 
    // 
    // public UnitController(UnitInfo unitInfo)
    // {
    //     this.unitInfo = unitInfo;
    // }

    public void TakeDamage(int damage)
    {
        print("TakeDamage: " + damage);
    }

    public void Heal(int amount)
    {
        print("Heal: " + amount);
    }

    public void ApplyModifier(IAction modifier)
    {
        print("ApplyModifier");
    }
}
