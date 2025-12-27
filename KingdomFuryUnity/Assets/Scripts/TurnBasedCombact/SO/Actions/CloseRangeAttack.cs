using System.Collections;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace TurnBasedCombact.SO.Actions
{
    [CreateAssetMenu(fileName = "CloseRangeAttack", menuName = "Scriptable Objects/CloseRangeAttack")]
    
    public class CloseRangeAttack : CombactActionBase
    {
        public PlayableAsset timeline;
    
        public override IEnumerator ExecuteAction(CombactUnit combactUnit, CombactInfo combactInfo)
        {
            Vector2 enemyPosition = combactInfo.enemyTargets[0].transform.position;
            Vector2 startingPosition = combactUnit.transform.position;
            Vector2 target = enemyPosition - (enemyPosition - startingPosition).normalized * 0.5f;
            
            yield return new WaitForSeconds(0.5f);
        
            yield return combactUnit.executor.MoveTo(target);

            CombactEventManager.istance.target = combactInfo.enemyTargets[0];
            CombactEventManager.istance.amount = 10;
                
            yield return combactUnit.executor.ExecuteTimeline(timeline);
            
            yield return combactUnit.executor.MoveTo(startingPosition);
        }
    }
}