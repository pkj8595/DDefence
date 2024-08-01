using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject obj) where T : UnityEngine.Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
            component = obj.AddComponent<T>();

        return component;
    }


    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static int ToInt(this Enum enumValue)
    {
        return Convert.ToInt32(enumValue);
    }

    private static readonly Dictionary<Enum, string> cacheEStr = new();
    public static string ToString(this Enum value)
    {
        if (cacheEStr.ContainsKey(value))
        {
            cacheEStr.Add(value, value.ToString());
        }
        return cacheEStr[value];
    }
    
}
