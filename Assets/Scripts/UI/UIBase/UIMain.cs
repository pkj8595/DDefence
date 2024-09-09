using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBase
{
    [SerializeField] private GameObject _btnNextPhase;
    [SerializeField] private Transform _relicParent;

    [SerializeField] private UI_RelicDesc _relicDesc;       //유물 설명 UI



    [Header("prefab")]
    [SerializeField] private UI_Relic _relicPrefab;

    [field : SerializeField] public UI_MoveResource UIMoveResource { get; set; }

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

    public void SetRelicUI()
    {

    }

}
