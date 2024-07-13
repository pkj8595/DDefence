using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utils
{

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go">최상위 부모</param>
    /// <param name="name">이름 없으면 비교하지 않음.</param>
    /// <param name="recursive">TRUE = 재귀적으로 찾을것인가 / FALSE = 하위만 찾을것인가</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive is false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }
        return null;
    }

    public static int CalculateTableNum(int tableNum)
    {
        return tableNum / 1000000;
    }

    public static int CalculateCategory(int tableNum)
    {
        return CalculateTableNum(tableNum) / 1000;
    }

    public static int CalculateTable(int tableNum, int category, int index)
    {
        return (tableNum * 1000000) + (category * 1000) + index;
    }

    public static int CalculateTableBaseNumber(int tableNum)
    {
        return tableNum - (tableNum % 1000);
    }



    public static int MakeFalg(params int[] flag)
    {
        int sum = 0;
        for (int i = 0; i < flag.Length; i++)
        {
            if (flag[i] > 9)
                break;

            sum += 1 << flag[i];
        }

        return sum;
    }

    /// <summary>
    /// 비트 연산자를 사용해 자리값이 1이 맞는지 확인한다.
    /// </summary>
    /// <param name="v">비교할 값</param>
    /// <param name="flag">비교할 값</param>
    /// <returns>
    /// true : 두 비트의 자리수가 1이 맞을 경우
    /// false : 두 비트의 자리수가 1이 아닐경우
    /// </returns>
    public static bool CompareFlag(int v ,int flag)
    {
        return (1 << v & 1 << flag) == 1;
    }

    

}
