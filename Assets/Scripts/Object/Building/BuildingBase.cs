using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour, ISelectedable
{
    [SerializeField] public GameObject _model;
    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    public Data.BuildingData Data { get; private set; }
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
        Data = data;
        if (Stat == null)
            Stat = gameObject.GetOrAddComponent<BuildingStat>();
        Stat.Init(data.tableNum, OnDead, OnDeadTarget);

        if (Data.productionTable != 0)
        {
            _production = gameObject.GetOrAddComponent<BuildingProduction>();
            _production.Init(Data.productionTable, this);
        }

        if (Data.baseSkill != 0)
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

    public void UpgradeBuilding()
    {
        if (Data.upgrade_goods_amount < Managers.Game.Inven.Goods[(Define.GoodsType)Data.upgrade_goods])
        {
            Managers.Game.Inven.Goods[(Define.GoodsType)Data.upgrade_goods] -= Data.upgrade_goods_amount;
        }

        Init(Data.upgradeNum);
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
        throw new System.NotImplementedException();
    }

    public void OnDeSelect()
    {
        throw new System.NotImplementedException();
    }

    public bool IsSelected()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
