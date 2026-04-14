using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace Systems.StateMachineSystem
{
    public class FSMContext : MonoBehaviour
    {
        private Dictionary<Type, Object> _objects = new Dictionary<Type, Object>();
        
        public void Add<T>(Object obj)
        {
            
            Type type = typeof(T);
            
            if (_objects.ContainsKey(type))
                _objects[type] = obj;
            else
                _objects.Add(type, obj);

        }
        
        public Object Get<T>()
        {
            return _objects[typeof(T)];
        }
    }
}