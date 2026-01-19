using System.Collections;
using Systems.CombactActionSystem;

public abstract class CombatActionBase : IAction
{
    public IEnumerator Execute(ActionContext context)
    {
        yield return Execute(context);
    }

    protected abstract IEnumerator Execute(CombatActionContext context);
}
