using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{

    public HashSet<IProductionable> _productionList = new HashSet<IProductionable>();

    public void RegisterProduction(IProductionable productionable)
    {
        _productionList.Add(productionable);
    }

    public void RemoveProduction(IProductionable productionable)
    {
        _productionList.Remove(productionable);
    }

    public void RunEndWave()
    {
        foreach(var production in _productionList)
        {
            production.EndWave();
        }
    }
}