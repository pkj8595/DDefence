using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase 
{
    private Transform _uiRoot;
    //List지만 stack처럼 활용한다.
    private List<UIBase> _uiStack = new List<UIBase>();

    public int baseSortingOrder = 0;
    public int sortingOrderAddValue = 100;

    public void Init(GameObject root)
    {
        base.Init();
        _uiRoot = new GameObject("@UIManager").transform;
        _uiRoot.parent = root.transform;
    }

    /// <summary>
    /// 팝업 생성 및 캐싱되어 있으면 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public UIBase ShowUI<T>(UIData uiData = null) where T : UIBase
    {
        //캐싱된 UI 찾기
        UIBase ui = GetUI<T>();
       
        if (ui == null)
        {
            ui = Managers.Resource.LoadUI<T>(_uiRoot);
            if (ui == null)
                return null;
            _uiStack.Add(ui);
        }
        else
        {
            _uiStack.Remove(ui);
            _uiStack.Add(ui);
        }

        UpdateSortingOrder();
        ui.SetUIBaseData();
        ui.Init(uiData);
        ui.UpdateUI();
        return ui;
    }

    public UIBase ShowUIPopup<T>(UIData uiData = null) where T : UIPopup
    {
        //캐싱된 UI 찾기
        UIBase ui = GetUI<T>();

        if (ui == null)
        {
            ui = Managers.Resource.LoadUIPopup<T>(_uiRoot);
            if (ui == null)
                return null;
            _uiStack.Add(ui);
        }
        else
        {
            _uiStack.Remove(ui);
            _uiStack.Add(ui);
        }
        UpdateSortingOrder();
        ui.SetUIBaseData();
        ui.Init(uiData);
        ui.UpdateUI();
        return ui;
    }

    /// <summary>
    /// 현재 켜져있는 UI의 sortingorder 갱신
    /// </summary>
    public void UpdateSortingOrder()
    {
        int activeCount = 0;
        for (int i = 0; i < _uiStack.Count; i++)
        {
            if (_uiStack[i].isActiveAndEnabled)
            {
                activeCount++;
                _uiStack[i].SetSortingOrder(baseSortingOrder + (activeCount * sortingOrderAddValue));
            }
        }
    }

    public void ColseUI<T>() where T : UIBase
    {
        UIBase targetUI = GetUI<T>();
        targetUI.Close();
    }

    public UIBase GetUI(string uiName)
    {
        for (int i = 0; i < _uiStack.Count; i++)
        {
            if (_uiStack[i].UIName == uiName)
                return _uiStack[i];
        }

        return null;
    }

    public UIBase GetUI<T>() where T : UIBase
    {
        for (int i = 0; i < _uiStack.Count; i++)
        {
            if (_uiStack[i] is T)
                return _uiStack[i];
        }

        return null;
    }

    public UIBase GetTopUI()
    {
        if (0 < _uiStack.Count)
            return _uiStack[_uiStack.Count - 1];
        else
            return null;
    }


    public void ShowToastMessage(string message)
    {
        var toast = ShowUIPopup<UIToastMessage>() as UIToastMessage;
        toast.SetMessage(message);
    }

    public override void Clear()
    {
        base.Clear();

        GameObject.Destroy(_uiRoot);
        _uiStack.Clear();
    }
}
