using System.Collections;
using Systems.CombactActionSystem.Model;
using UnityEngine;

namespace Systems.CombactActionSystem.ActionImpl
{
    public class SetAnimatorBool : IAction
    {
        private Animator _animator;
        private IAction _wrappedAction;
        private string _name;

        public SetAnimatorBool(IAction action, Animator animator, string name)
        {
            _animator = animator;
            _wrappedAction = action;
            _name = name;
        }

        public IEnumerator Execute(CombactContext cbContext)
        {
            _animator.SetBool(_name, true);
            yield return _wrappedAction.Execute(cbContext);
            _animator.SetBool(_name, false);

        }
    }
}