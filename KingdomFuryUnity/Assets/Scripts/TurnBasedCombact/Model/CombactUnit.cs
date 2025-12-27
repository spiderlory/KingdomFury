using System.Collections.Generic;
using TurnBasedCombact.Interfaces;
using TurnBasedCombact.SO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Serialization;

namespace TurnBasedCombact.Model
{
    public class CombactUnit : MonoBehaviour
    {
        public IDirectiveExecutor executor;
        public PlayableDirector director;
        
        public CombactUnitDataSO unitData;

        private void Start()
        {
            // Components Init
            director = GetComponent<PlayableDirector>();
            executor = GetComponent<IDirectiveExecutor>();
        }

        public void ExecuteAction(int i, CombactInfo combactInfo)
        {
            StartCoroutine(unitData.actions[i].ExecuteAction(this, combactInfo));
        }

        public void HitAnimation()
        {
            print("hitAnimation");
            StartCoroutine(executor.ExecuteClip(unitData.hitClip));
        }
    }
}