using System;
using System.Collections;
using Systems.CombactActionSystem.Model;
using UnityEngine;
using UnityEngine.Playables;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class MoveTo : IAction
    {
        private Func<CombactContext, Vector2> _targetResolver;
        float _speed;
        
        public MoveTo(Func<CombactContext, Vector2> targetResolver)
        {
            _targetResolver = targetResolver;
            _speed = 2;
        }
        
        public IEnumerator Execute(CombactContext cbContext)
        {
            Transform transform = cbContext.CurrentPlayer.transform;

            Vector2 targetPosition = _targetResolver(cbContext);
            Debug.Log("MOVING TO: " + targetPosition);
            
            
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