using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : ManagerBase 
{
    //List지만 stack처럼 활용한다.
    public List<UIBase> uiStack = new List<UIBase>();

    public int bestSortingOrder = 0;
    public int changeSortingOrderValue = 100;

    /// <summary>
    /// 팝업 생성 및 캐싱되어 있으면 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public UIBase ShowUI<T>(UIData uiData) where T : UIBase
    {
        //캐싱된 UI 찾기
        UIBase ui = GetUI<T>();
        if (ui != null)
            return ui;

        if (ui == null) 
        {
            
            return null;
        } 
        uiStack.Add(ui);
        ui.SetUIBaseData(bestSortingOrder);
        ui.Init(uiData);
        ui.UpdateUI();
        return ui;
    }

    public void RemoveUI<T>() where T : UIBase
    {
        UIBase targetUI = GetUI<T>();
        targetUI.Hide();
        uiStack.Remove(targetUI);
    }

    public UIBase GetUI(string uiName)
    {
        for (int i = 0; i < uiStack.Count; i++)
        {
            if (uiStack[i].UIName == uiName)
                return uiStack[i];
        }
        return null;
    }

    public UIBase GetUI<T>() where T : UIBase
    {
        for (int i = 0; i < uiStack.Count; i++)
        {
            if (uiStack[i] is T)
                return uiStack[i];
        }
        return null;
    }


}
