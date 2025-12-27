using System.Collections;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace TurnBasedCombact.Interfaces
{
    public interface IDirectiveExecutor
    {
        public IEnumerator MoveTo(Vector2 targetPosition);
        public IEnumerator ExecuteTimeline(PlayableAsset timeline);
        public IEnumerator ExecuteClip(AnimationClip clip);
    }
}

