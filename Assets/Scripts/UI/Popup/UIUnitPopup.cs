using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitPopup : UIPopup
{
    [SerializeField] private UI_UnitInfo _unitInfo;
    Unit _unit;

    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        UIUnitData data = uiData as UIUnitData;
        _unit = data.unitGameObject;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();

        if (_unit is PawnBase)
        {
            _unitInfo.SetPawnBase(_unit as PawnBase);
        }
        else
        {
            _unitInfo.SetBuildingBase(_unit as BuildingBase);
        }
    }

}
