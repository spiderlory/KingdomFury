using System.Collections;

namespace Systems.CombactActionSystem
{
    public interface IAction
    {
        public IEnumerator Execute(ActionContext actionContext);
    }
}