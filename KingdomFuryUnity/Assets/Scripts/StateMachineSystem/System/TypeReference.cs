using System;
using UnityEngine;

[Serializable]
public class TypeReference
{
    [SerializeField] public string typeName;
    
    public Type Type
    {
        get
        {
            return string.IsNullOrEmpty(typeName)? null : Type.GetType(typeName);
        }
        set
        {
            typeName = value?.AssemblyQualifiedName;
        }
    }
}
