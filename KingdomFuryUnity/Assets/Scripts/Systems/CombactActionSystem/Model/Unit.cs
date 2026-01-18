using System.Collections.Generic;
using UnityEngine;

namespace Systems.CombactActionSystem.Model
{
    public class Unit: MonoBehaviour
    {
        List<IAction> actions = new List<IAction>();
        UnitInfo unitInfo;
        
        
        public Unit(UnitInfo unitInfo)
        {
            this.unitInfo = unitInfo;
        }

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
}