using System;
using System.Collections;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class MoveTo : CombatActionBase
    {
        private Func<ActionContext, Vector2> _targetResolver;
        float _speed;
        
        public MoveTo(Func<ActionContext, Vector2> targetResolver)
        {
            _targetResolver = targetResolver;
            _speed = 2;
        }
        
        protected override IEnumerator Execute(CombatActionContext context)
        {
            Transform transform = context.Player.transform;

            Vector2 targetPosition = _targetResolver(context);
            
            while (true)
            {
                Vector2 currentPosition = transform.position;
                Vector2 deltaPosition = (targetPosition - currentPosition);
    
                Vector2 translation = deltaPosition.normalized * (Time.deltaTime * _speed);
    
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
        }
    }
}