using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProductionable
{
    public void EndWave();
}

public class BuildingProduction : MonoBehaviour, IProductionable
{
    BuildingBase _buildingBase;
    Data.ProductionData _data;

    private void OnEnable()
    {
        Managers.Game.RegisterProduction(this);
    }

    private void OnDisable()
    {
        Managers.Game.RemoveProduction(this);
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


}
