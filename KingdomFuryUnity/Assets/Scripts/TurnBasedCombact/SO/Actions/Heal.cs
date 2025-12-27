using System.Collections;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace TurnBasedCombact.SO.Actions
{
    [CreateAssetMenu(fileName = "", menuName = "Scriptable Objects/Heal")]
    public class Heal : CombactActionBase
    {
        public PlayableAsset timeline;
        
        public override IEnumerator ExecuteAction(CombactUnit combactUnit, CombactInfo combactInfo)
        {
            yield return new WaitForSeconds(0.5f);
            
            yield return combactUnit.executor.ExecuteClip(combactUnit.unitData.healClip);
            
            yield return new WaitForSeconds(0.5f);
        }
    }
}