using System.Collections;
using System.Collections.Generic;
using Systems.CombactActionSystem.ActionImpl;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Systems.CombactActionSystem.Model
{
    public class TimelineAction : IAction
    {
        PlayableDirector _director;
        PlayableAsset _playableAsset;
        
        Queue<IAction> combactEventsQueue;

        private bool _continue;

        public TimelineAction(PlayableDirector director, PlayableAsset playableAsset)
        {
            _director = director;
            _playableAsset = playableAsset;
        }

        public IEnumerator Execute(CombactContext cbContext)
        {
            SignalDispatcher.test += NextAction;
            
            _director.playableAsset = _playableAsset;
            _director.Play();
            
            yield return WaitNextSignal();
            IAction damage = new DamageTarget((ctx => ctx.EnemyTargets[0].GetComponent<Unit>()));
            yield return damage.Execute(cbContext);
            
            yield return WaitNextSignal();
            
            yield return WaitTimelineEnd();
            Debug.Log("END");
            
            SignalDispatcher.test -= NextAction;
        }

        private void NextAction()
        {
            _continue = true;
        }

        IEnumerator WaitNextSignal()
        {
            yield return new WaitUntil(() => _continue);
            _continue = false;
        }
        
        IEnumerator WaitTimelineEnd()
        {
            yield return new WaitUntil(() => !_director.state.Equals(PlayState.Playing));
        }

    }
}