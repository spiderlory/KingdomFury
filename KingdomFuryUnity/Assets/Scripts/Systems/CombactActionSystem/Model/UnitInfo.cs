using UnityEngine;

namespace Systems.CombactActionSystem.Model
{
    public class UnitInfo
    {
        public GameObject GameObject { get; }

        public UnitStats Stats { get; }

        public UnitCurrentStats CurrentStats { get; }

        public UnitInfo(GameObject gameObject, UnitStats stats, UnitCurrentStats currentStats)
        {
            GameObject = gameObject;
            Stats = stats;
            CurrentStats = currentStats;
        }
    }
}