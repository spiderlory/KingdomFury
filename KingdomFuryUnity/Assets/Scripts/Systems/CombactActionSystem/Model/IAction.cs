using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Systems.CombactActionSystem.Model
{
    public interface IAction
    {
        public IEnumerator Execute(CombactContext cbContext);
        // public _wrappedAction;
        // private IAction _prev;
        // private lifeTime;
        // private modifierType;
// 
        // public UpdateLifeTime();
        // private Destroy();
    }
}