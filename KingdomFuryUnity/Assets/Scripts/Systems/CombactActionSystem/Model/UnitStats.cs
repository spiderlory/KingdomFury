using UnityEngine;

namespace Systems.CombactActionSystem.Model
{
    [CreateAssetMenu(fileName = "UnitStats", menuName = "Scriptable Objects/UnitStats")]
    public class UnitStats : ScriptableObject
    {
        [SerializeField] private int _health;
        [SerializeField] private int _impetus;

        [SerializeField] private int _attack;
        [SerializeField] private int _defence;

        [SerializeField] private int _speed;
        [SerializeField] private int _affinity;

        public int Health => _health;

        public int Impetus => _impetus;

        public int Attack => _attack;

        public int Defence => _defence;

        public int Speed => _speed;

        public int Affinity => _affinity;
    }
}