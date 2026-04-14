using System;
using System.Collections;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class DamageTarget : CombatActionBase
    {
        private Func<CombatActionContext, UnitController> _targetResolver;

        public DamageTarget(Func<CombatActionContext, UnitController> targetResolver)
        {
            _targetResolver = targetResolver;
        }


        protected override IEnumerator Execute(CombatActionContext context)
        {
            UnitController enemy = _targetResolver(context);
            
            int damage = (int) Math.Floor(10f);
            
            enemy.TakeDamage(damage);
            return null;
        }
    }
}