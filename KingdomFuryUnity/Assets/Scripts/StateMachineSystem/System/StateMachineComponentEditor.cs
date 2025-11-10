using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

[CustomEditor(typeof(StateMachineBlueprint))]
public class StateMachineComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var comp = (StateMachineBase) target;

        // Trova tutte le classi che ereditano da State
        Type baseType = typeof(StateMachineBlueprint.State);
        Type[] derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract)
            .ToArray();

        string[] options = derivedTypes.Select(t => t.Name).ToArray();
        int currentIndex = Array.FindIndex(derivedTypes, t => t == comp.test);

        int selectedIndex = EditorGUILayout.Popup("Selected State", currentIndex, options);

        if (selectedIndex >= 0 && selectedIndex < derivedTypes.Length)
        {
            comp.GetType().GetField("selectedState", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(comp, new TypeReference { Type = derivedTypes[selectedIndex] });
        }

        DrawDefaultInspector();
    }
}