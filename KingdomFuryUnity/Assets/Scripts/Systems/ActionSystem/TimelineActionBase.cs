using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Systems.CombactActionSystem
{
    public abstract class TimelineActionBase : IAction
    {
        PlayableAsset _playableAsset;
        
        Queue<IAction> _combactEventsQueue;

        private bool _continue;

        protected TimelineActionBase(PlayableAsset playableAsset)
        {
            _playableAsset = playableAsset;
        }
        
        public IEnumerator Execute(ActionContext actionContext)
        {
            SignalDispatcher.nextAction += NextAction;
            
            yield return OnExecute(actionContext);
            
            SignalDispatcher.nextAction -= NextAction;
        }
        
        protected abstract IEnumerator OnExecute(ActionContext actionContext);

        private void NextAction()
        {
            _continue = true;
        }

        protected IEnumerator WaitNextSignal()
        {
            yield return new WaitUntil(() => _continue);
            _continue = false;
        }
        
        protected IEnumerator WaitTimelineEnd(PlayableDirector director)
        {
            yield return new WaitUntil(() => !director.state.Equals(PlayState.Playing));
        }

    }
}