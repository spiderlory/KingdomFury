using System.Collections;
using System.Collections.Generic;

namespace Systems.CombactActionSystem
{
    public class ActionRunner
    {
        private List<IAction> _actionsList;

        public virtual IEnumerator ExecuteActions(ActionContext actionContext)
        {
            foreach (IAction action in _actionsList)
            {
                yield return action.Execute(actionContext);
            }
        }
    }
}