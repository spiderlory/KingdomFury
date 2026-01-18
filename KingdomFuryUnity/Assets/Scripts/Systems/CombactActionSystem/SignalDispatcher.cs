using System;
using UnityEngine;
namespace Systems.CombactActionSystem
{
    public class SignalDispatcher : MonoBehaviour
    {
        public static event Action test;

        public void DispatchNext()
        {
            print("DispatchNext");
            test?.Invoke();
        }
    }
}