using System.Collections;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class SetAnimatorBool : CombatActionBase
    {
        private Animator _animator;
        private IAction _wrappedAction;
        private string _name;

        public SetAnimatorBool(IAction action, string name)
        {
            _wrappedAction = action;
            _name = name;
        }

        protected override IEnumerator Execute(CombatActionContext context)
        {
            Animator animator = context.PlayerComponents.Animator;
            animator.SetBool(_name, true);
            yield return _wrappedAction.Execute(context);
            animator.SetBool(_name, false);
        }
    }
}