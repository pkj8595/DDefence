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

    public static void ResetTransform(this Transform transform)
    {
        transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// 팀이 다를경우 타겟 타입을 Enemy로 반환
    /// </summary>
    /// <param name="pawnTeam"></param>
    /// <returns></returns>
    public static Define.ETargetType GetTargetType(this IDamageable unit, Define.ETeam pawnTeam)
    {
        if (unit.Team == pawnTeam)
            return Define.ETargetType.Ally;
        else
            return Define.ETargetType.Enemy;
    }

    /// <summary>
    /// 적 
    /// </summary>
    /// <param name="searchRange"></param>
    /// <returns></returns>
    public static IDamageable SearchTarget(this IDamageable transform, float searchRange, Define.ETargetType targetType)
    {
        if (Define.ETargetType.Self == targetType)
            return transform;

        int layerTarget = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
        Collider[] colliders = Physics.OverlapSphere(transform.GetTransform().position, searchRange, layerTarget);

        foreach (var collider in colliders)
        {
            IDamageable unit = collider.GetComponent<IDamageable>();
            if (unit != null && !unit.IsDead() && unit.GetTargetType(transform.Team) == targetType)
            {
                return unit;
            }
        }
        return null;
    }

}
