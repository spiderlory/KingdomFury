using UnityEngine;

namespace TurnBasedCombat.Model.Stats
{
    public class MaxHealth : StatBase
    {
        public MaxHealth(float baseValue) : base(baseValue) { }

        protected override void OnAddToValue(float amount)
        {
            _currentValue = Mathf.Clamp(_currentValue + amount, 0, _baseValue);
        }
    }
}