using UnityEngine;

namespace TurnBasedCombat.Model.Stats
{
    public abstract class StatBase
    {
        protected float _baseValue;
        protected float _currentValue;
        protected float _deltaValue;

        public StatBase(float baseValue)
        {
            _baseValue = baseValue;
            _currentValue = baseValue;
            _deltaValue = 0;
        }

        public float GetBaseValue()
        {
            return _baseValue;
        }

        public float GetCurrentValue()
        {
            return _currentValue;
        }

        public float GetDeltaValue()
        {
            return _deltaValue;
        }


        public void AddToValue(float amount)
        {
            OnAddToValue(amount);
            _deltaValue = _baseValue - _currentValue;
        }

        protected abstract void OnAddToValue(float amount);

    }
}

