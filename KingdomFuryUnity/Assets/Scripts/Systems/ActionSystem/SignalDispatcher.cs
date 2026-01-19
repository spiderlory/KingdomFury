using System;
using UnityEngine;

namespace Systems.CombactActionSystem
{
    public class SignalDispatcher : MonoBehaviour
    {
        public static event Action nextAction;

        public void DispatchNext()
        {
            print("DispatchNext");
            nextAction?.Invoke();
        }
    }
}