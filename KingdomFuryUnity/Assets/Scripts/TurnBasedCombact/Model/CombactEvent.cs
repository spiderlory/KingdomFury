using System.Collections.Generic;

namespace TurnBasedCombact.Model
{
    public class CombactEvent
    {
        public CombactEventType CombactEventType;
        public List<CombactUnit> targets;
        public float value;

        public CombactEvent(CombactEventType combactEventType, CombactUnit target, float value)
        {
            this.CombactEventType = combactEventType;
            this.targets = new List<CombactUnit>();
            this.targets.Add(target);
            
            this.value = value;
        }
        
        public CombactEvent(CombactEventType combactEventType, List<CombactUnit> targets, float value)
        {
            this.CombactEventType = combactEventType;
            this.targets = targets;
            this.value = value;
        }
    }
}