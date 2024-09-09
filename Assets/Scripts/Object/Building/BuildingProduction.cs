using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProduction : MonoBehaviour, IWaveEvent
{
    BuildingBase _buildingBase;
    Data.ProductionData _data;
    int _waveCount;

    public void Init(int tableNum, BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _data = Managers.Data.ProductionDict[tableNum];
        _waveCount = 0;
    }

    public void EndWave()
    {
        if (_buildingBase.Stat.IsDead)
            return;

        _waveCount++;

        if (_data.waveCount <= _waveCount)
        {
            _waveCount -= _data.waveCount;
            if (Utils.CalculateTableNum(_data.itemNum) == Data.GoodsData.Table)
            {
                Managers.Game.Inven.AddMoveItem(transform.position, (Define.EGoodsType)_data.itemNum, _data.itemAmount);
            }
            else
            {
                Managers.Game.Inven.AddItem(_data.itemNum, _data.itemAmount);
            }

        }
    }

    public void ReadyWave()
    {
        
    }
}
