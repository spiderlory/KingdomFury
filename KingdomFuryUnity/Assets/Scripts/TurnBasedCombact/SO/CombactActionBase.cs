using System.Collections;
using System.Collections.Generic;
using TurnBasedCombact.Interfaces;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace TurnBasedCombact.SO
{
    public abstract class CombactActionBase : ScriptableObject
    {
        public string actionName;
        public static CombactEventManager combactEventManager;
        
        public abstract IEnumerator ExecuteAction(CombactUnit combactUnit, CombactInfo combactInfo);
    }
}