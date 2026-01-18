using System;
using System.Collections;
using TurnBasedCombact.Interfaces;
using TurnBasedCombact.Model;
using TurnBasedCombact.SO;
using UnityEngine;
using UnityEngine.Playables;

namespace TurnBasedCombact.Model.Mage
{
    public class MageDExecutor : MonoBehaviour, IDirectiveExecutor
    {
        private CombactUnit combactUnit;
        private Transform transform;
        private PlayableDirector director;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private CombactUnitDataSO _unitData;
        private AnimatorOverrideController _aoc;

        private void Start()
        {
            combactUnit = GetComponent<CombactUnit>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            _aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
            
            _aoc["Idle"] = combactUnit.unitData.idleClip;
            _aoc["Walking"] = combactUnit.unitData.walkingClip;
            
            animator.runtimeAnimatorController = _aoc;
            
            
            transform = combactUnit.transform;
            director = combactUnit.director;
            _unitData = combactUnit.unitData;
        }

        public IEnumerator MoveTo(Vector2 targetPosition)
        {
            animator.SetBool("IsWalking", true);

            spriteRenderer.flipX = targetPosition.x < transform.position.x;
 
            while (true)
            {
                Vector2 currentPosition = transform.position;
                Vector2 deltaPosition = (targetPosition - currentPosition);
    
                Vector2 translation = deltaPosition.normalized * (Time.deltaTime * _unitData.stats.speed);
    
                if (translation.magnitude < deltaPosition.magnitude)
                {
                    transform.Translate(translation);
                    yield return null;
                }
                else
                {
                    transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
                    break;
                }
            }
            animator.SetBool("IsWalking", false);
            spriteRenderer.flipX = false;
        }

        public IEnumerator ExecuteTimeline(PlayableAsset timeline)
        {
            if (director.state == PlayState.Playing)
            {
                director.Stop();
            }
        
            director.playableAsset = timeline;
            director.Play();

            // Wait timeline's end
            while (director.state == PlayState.Playing)
            {
                yield return null; // pausa un frame e riprende
            }
        }

        public IEnumerator ExecuteClip(AnimationClip clip)
        {
            _aoc["Action"] = clip;
            
            animator.SetTrigger("Action");
            
            // Aspetta che entri nello stato Action
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Action"));

            // Aspetta che l'animazione finisca
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f &&
                !animator.IsInTransition(0)
            );
        }
    }
}