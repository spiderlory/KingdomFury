using System.Collections.Generic;
using TurnBasedCombact.Model;
using UnityEngine;

namespace TurnBasedCombact.SO
{
    [CreateAssetMenu(fileName = "CombactUnitDataSO", menuName = "Scriptable Objects/CombactUnitDataSO")]
    public class CombactUnitDataSO : ScriptableObject
    {
        
        public CombactUnitStats stats;
        
        // Actions Clips
        public AnimationClip hitClip;
        public AnimationClip healClip;
        public AnimationClip deathClip;
        
        // Animator Clips
        public AnimationClip idleClip;
        public AnimationClip walkingClip;
        
        public List<CombactActionBase>  actions;
    }
}
