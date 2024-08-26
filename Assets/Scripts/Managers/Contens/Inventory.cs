using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    private Dictionary<Define.GoodsType, int> _goods = new();
    public Dictionary<Define.GoodsType, int> Goods { get => _goods; set => _goods = value; }

    public void Init()
    {
        foreach (var data in Managers.Data.GoodsDict)
        {
            _goods.Add((Define.GoodsType)data.Key, 0);
        }
    }
}
