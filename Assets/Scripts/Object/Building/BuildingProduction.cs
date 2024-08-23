using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProduction : MonoBehaviour
{
    BuildingBase _buildingBase;
    Data.ProductionData _data;
    public void Init(int tableNum, BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _data = Managers.Data.ProductionDict[tableNum];
    }

    public void EndWave()
    {
        if (_buildingBase.IsDead())
            return;

        Managers.Game.Goods[(Define.GoodsType)_data.itemNum] += _data.itemAmount;
    }


}
