using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : BuildingBase
{
    public GameObject buildingPrefab;

    public override void Init(BuildingData data)
    {
        base.Init(data);


    }

    public void WaveStart()
    {

    }

    public void WaveEnd()
    {
        if (IsDead())
            return;

    }

}
