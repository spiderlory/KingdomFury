namespace Systems.CombactActionSystem
{
    public abstract class ActionContext
    {
        public PlayerComponentsContext PlayerComponents { get; }

        protected ActionContext(PlayerComponentsContext playerComponents)
        {
            PlayerComponents = playerComponents;
        }
    }
}