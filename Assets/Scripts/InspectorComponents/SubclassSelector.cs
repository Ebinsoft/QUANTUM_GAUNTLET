using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SubclassSelector
{
    public static SerializedClass Dropdown<T>(string label, SerializedClass current)
    {
        List<Type> subclasses = typeof(T).Assembly.GetTypes()
            .Where(type => type.IsSubclassOf(typeof(T)))
            .ToList();

        if (current == null)
        {
            return new SerializedClass(subclasses[0]);
        }

        string[] names = subclasses.Select(b => b.ToString()).ToArray();
        int currentIdx = Math.Max(0, Array.IndexOf(names, current.className));

        currentIdx = EditorGUILayout.Popup(label, currentIdx, names);
        return new SerializedClass(subclasses[currentIdx]);
    }
}

[Serializable]
public class SerializedClass
{
    public string className;
    public string qualifiedName;

    public SerializedClass(Type classType)
    {
        this.className = classType.ToString();
        this.qualifiedName = classType.AssemblyQualifiedName;
    }

    public System.Object CreateInstance()
    {
        return Activator.CreateInstance(Type.GetType(qualifiedName));
    }
}