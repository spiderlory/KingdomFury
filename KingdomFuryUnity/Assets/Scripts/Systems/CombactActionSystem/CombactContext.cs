using System.Collections.Generic;
using Systems.CombactActionSystem.Model;
using UnityEngine;

namespace Systems.CombactActionSystem
{
    public class CombactContext
    {
        public GameObject CurrentPlayer { get; }
        public UnitInfo CurrentPlayerInfo => CurrentPlayer.GetComponent<UnitInfo>();
        public List<GameObject> EnemyTargets { get; }
        public List<GameObject> AllTargets { get; }

        public float damageMultiplier;

        public float healMultiplier;

        public CombactContext(GameObject currentPlayer, List<GameObject> enemyTargets, List<GameObject> allTargets)
        {
            CurrentPlayer = currentPlayer;
            EnemyTargets = enemyTargets;
            AllTargets = allTargets;
            
            damageMultiplier = 1f;
            healMultiplier = 1f;
        }
    }
}