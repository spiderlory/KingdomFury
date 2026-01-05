using System;
using System.Collections.Generic;
using System.Linq;
using TurnBasedCombact.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class CombactEventManager : MonoBehaviour
{
    private Queue<CombactEvent> combactEventsQueue = new Queue<CombactEvent>();
    
    public static CombactEventManager instance = null;
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public void ExecuteNext()
    {
        ExecuteEvent(combactEventsQueue.Dequeue());
    }

    public void ExecuteAll()
    {
        while (combactEventsQueue.Count > 0)
        {
            ExecuteEvent(combactEventsQueue.Dequeue());
        }
    }

    private void ExecuteEvent(CombactEvent combactEvent)
    {
        switch (combactEvent.CombactEventType)
        {
            case CombactEventType.Heal:
                Heal(combactEvent);
                break;
            case CombactEventType.Damage:
                Damage(combactEvent);
                break;
        }
    }

    public void ClearQueue()
    {
        combactEventsQueue.Clear();
    }

    public void SetEventsQueue(Queue<CombactEvent> queue)
    {
        combactEventsQueue = queue;
    }
    
    public void Heal(CombactEvent combactEvent)
    {
        print("healed for");
    }

    public void Damage(CombactEvent combactEvent)
    {
        float amount = combactEvent.value;
        
        print("hit:" + amount.ToString());

        combactEvent.targets.ForEach(x => x.HitAnimation());
    }
}
