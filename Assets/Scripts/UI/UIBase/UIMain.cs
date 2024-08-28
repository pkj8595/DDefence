using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private GameObject _btnNextPhase;

    public override void SetUIBaseData()
    {
        base.SetUIBaseData();
    }

    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        
    }
    
    public override void UpdateUI()
    {
        base.UpdateUI();
    }
    public override void Close()
    {
        base.Close();
    }

    public void OnClickNextPhase()
    {
        Managers.Game.NextPhase();
        _btnNextPhase.SetActive(false);
    }

    public void ShowBtnNextPhase()
    {
        _btnNextPhase.SetActive(true);
    }

}
