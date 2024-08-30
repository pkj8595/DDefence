using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Inventory 
{
    public Dictionary<int, int> _itemDic = new Dictionary<int, int>();
    public Action<int> OnItemAmountChanged { get; set; }

    public void Init()
    {
        foreach (var data in Managers.Data.GoodsDict)
        {
            _itemDic.Add(data.Key, 0);
        }
        
    }

    public void AddItem(int itemNum, int amount)
    {
        if (!_itemDic.ContainsKey(itemNum))
        {
            _itemDic.Add(itemNum, 0);
            return;
        }

        _itemDic[itemNum] += amount;
        OnItemAmountChanged?.Invoke(itemNum);
    }

    public int GetItem(int itemNum)
    {
        if (!_itemDic.ContainsKey(itemNum))
        {
            _itemDic.Add(itemNum, 0);
        }

        return _itemDic[itemNum];
    }

    /// <summary>
    /// 아이템이 있는지 체크
    /// </summary>
    /// <param name="itemNum"></param>
    /// <param name="itemAmount"></param>
    /// <returns>아이템의 수량이 충분하면 true</returns>
    public bool CheckItem(int itemNum, int itemAmount)
    {
        if (_itemDic.ContainsKey(itemNum))
        {
            if (itemAmount <= _itemDic[itemNum])
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 아이템이 있는지 체크 후 소모
    /// </summary>
    /// <param name="itemNum"></param>
    /// <param name="itemAmount"></param>
    /// <returns>아이템과 수량이 충분하면 소모하다 true 리턴</returns>
    public bool SpendItem(int itemNum, int itemAmount)
    {
        if (CheckItem(itemNum, itemAmount))
        {
            _itemDic[itemNum] -= itemAmount;
            OnItemAmountChanged?.Invoke(itemNum);
            return true;
        }
        return false;
    }

    public void SpendWaveCost(Define.EGoodsType goodsType,int itemAmount)
    {
        if (!_itemDic.ContainsKey((int)goodsType))
        {
            _itemDic.Add((int)goodsType, 0);
        }
        _itemDic[(int)goodsType] -= itemAmount;
        OnItemAmountChanged?.Invoke((int)goodsType);
    }



}
