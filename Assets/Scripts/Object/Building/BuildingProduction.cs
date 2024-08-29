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

    private void OnEnable()
    {
        Managers.Game.RegisterWaveObject(this);
    }

    private void OnDisable()
    {
        Managers.Game.RemoveWaveObject(this);
    }

    public void Init(int tableNum, BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _data = Managers.Data.ProductionDict[tableNum];
    }

    public void EndWave()
    {
        if (_buildingBase.Stat.IsDead)
            return;

        Managers.Game.Inven.AddItem(_data.itemNum, _data.itemAmount);
    }

    public void ReadyWave()
    {
        
    }
}
