using System.Collections.Generic;
using Systems.CombactActionSystem;
using UnityEngine;

public class CombatActionContext : ActionContext
{
    public GameObject Player { get; }
    
    public CombatActionContext(PlayerComponentsContext playerComponents, GameObject player) : base(playerComponents)
    {
        Player = player;
    }
}
