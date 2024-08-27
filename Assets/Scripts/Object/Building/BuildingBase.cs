using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour, ISelectedable
{
    [SerializeField] public GameObject _model;
    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    public Data.BuildingData BuildingData { get; private set; }
    public  BuildingStat Stat { get; private set; }
    protected BuildingProduction _production;
    protected BuildingSkill _skill;
    protected BuildingDamageable _damageable;

    public int BuildingTableNum;

    public void Start()
    {
        Init(BuildingTableNum);
    }

    public virtual void Init(int tableNum)
    {
        Init(Managers.Data.BuildingDict[tableNum]);
    }

    public virtual void Init(Data.BuildingData data)
    {
        //todo 건물 테이블 수정하고 구현
        BuildingData = data;
        if (Stat == null)
            Stat = gameObject.GetOrAddComponent<BuildingStat>();
        Stat.Init(data.tableNum, OnDead, OnDeadTarget);

        if (BuildingData.productionTable != 0)
        {
            _production = gameObject.GetOrAddComponent<BuildingProduction>();
            _production.Init(BuildingData.productionTable, this);
        }

        if (BuildingData.baseSkill != 0)
        {
            _skill = gameObject.GetOrAddComponent<BuildingSkill>();
            _skill.Init(this);
        }

        if (data.isDamageable)
        {
            _damageable = gameObject.GetOrAddComponent<BuildingDamageable>();
            _damageable.Init(this);

            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.AddUnit(_damageable);
        }

        
    }

    public void OnClickUpgradeBuilding()
    {
        if (Managers.Game.Inven.SpendItem(BuildingData.upgrade_goods, BuildingData.upgrade_goods_amount))
        {
            Init(BuildingData.upgradeNum);
            return;
        }

        Managers.UI.ShowToastMessage("업그레이드 비용이 부족합니다.");
    }

   
    public void OnDeadTarget()
    {

    }

    private void OnDestroy()
    {
        if (_damageable != null)
        {
            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.RemoveUnit(_damageable);
        }
    }

    public void OnDead()
    {
        if (_damageable != null)
        {
            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.SetActive(_damageable, false);
        }

        Managers.Effect.PlayAniEffect("SmallStingHit", transform.position);
        gameObject.SetActive(false);
    }

    #region ISelectable
    public void OnSelect()
    {
    }

    public void OnDeSelect()
    {
    }

    public bool IsSelected()
    {
        return false;
    }
    #endregion

}
