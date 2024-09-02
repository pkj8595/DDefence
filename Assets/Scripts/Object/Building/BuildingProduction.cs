using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaveEvent
{
    public void EndWave();
    public void ReadyWave();
}

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
            Managers.Game.Inven.AddItem(_data.itemNum, _data.itemAmount);
        }
    }

    public void ReadyWave()
    {
        
    }
}
