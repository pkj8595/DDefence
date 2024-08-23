using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateBarGroup : UIBase
{

    public Dictionary<IDamageable, UI_HPbar> _dicUnit = new(); // 체력바를 가진 오브젝트들
    public Queue<UI_HPbar> _stateBarPool = new(); // hp바 풀링
    public UI_HPbar _stateBarPrefab; //hp 프리팹


    public override void Init(UIData uiData)
    {
        base.Init(uiData);
        _stateBarPrefab.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        foreach (var unit in _dicUnit)
        {
            if (unit.Key == null || unit.Key.IsDead())
            {
                unit.Value.gameObject.SetActive(false);
                continue;
            }

            // 화면에서 보이는지 여부 확인
            Vector3 worldPosition = unit.Key.GetTransform().position + unit.Key.StateBarOffset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

            //화면에 있다면 없다면
            if (screenPosition.z > 0 &&
                screenPosition.x > 0 && screenPosition.x < Screen.width &&
                screenPosition.y > 0 && screenPosition.y < Screen.height)
            {
                //화면에 노출 된다면
                unit.Value.gameObject.SetActive(true);
                unit.Value.OnUpdatePosition(screenPosition);
            }
            else
            {
                unit.Value.gameObject.SetActive(false);
            }
        }
    }


    public void AddUnit(IDamageable unit)
    {
        if (_dicUnit.ContainsKey(unit))
            return;
        var stateBar = GetOrCreateStateBar();
        stateBar.Init(unit);
        _dicUnit.Add(unit, stateBar);
    }


    public void RemoveUnit(IDamageable unit)
    {
        DeActiveStatebar(_dicUnit[unit]);
        _dicUnit.Remove(unit);
    }

    public void SetActive(IDamageable unit, bool isActive)
    {
        if (_dicUnit.TryGetValue(unit, out UI_HPbar value))
        {
            value.gameObject.SetActive(isActive);
        }
    }

    private UI_HPbar GetOrCreateStateBar()
    {
        UI_HPbar ret;
        if (_stateBarPool.Count > 0)
            ret = _stateBarPool.Dequeue();
        else
            ret = Instantiate(_stateBarPrefab, transform);

        ret.gameObject.SetActive(true);
        return ret;
    }

    private void DeActiveStatebar(UI_HPbar stateBar)
    {
        stateBar.Clear();
        stateBar.gameObject.SetActive(false);
        _stateBarPool.Enqueue(stateBar);
    }


}
