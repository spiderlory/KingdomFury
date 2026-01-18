using System;
using System.Collections;
using Systems.CombactActionSystem.Model;
using Unity.Multiplayer.PlayMode;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class DamageTarget : IAction
    {
        private Func<CombactContext, Unit> _targetResolver;

        public DamageTarget(Func<CombactContext, Unit> targetResolver)
        {
            _targetResolver = targetResolver;
        }


        public IEnumerator Execute(CombactContext cbContext)
        {
            Unit enemy = _targetResolver(cbContext);
            
            int damage = (int) Math.Floor(10 * cbContext.damageMultiplier);
            
            enemy.TakeDamage(damage);
            return null;
        }
    }
}