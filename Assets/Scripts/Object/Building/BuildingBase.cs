using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour, IDamageable, ISelectedable
{
    [SerializeField] public GameObject _model;
    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    [SerializeField] private Vector3 BuildingStateBarOffset;
    public Vector3 StateBarOffset => BuildingStateBarOffset;

    public Data.BuildingData Data { get; private set; }
    public  BuildingStat Stat { get; private set; }
    protected BuildingProduction _production;
    protected BuildingSkill _skill;

    public int BuildingTableNum;

    public void Awake()
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

        UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
        uiStatebarGroup.AddUnit(this);
    }

    public void UpgradeBuilding()
    {
        if (Data.upgrade_goods_amount < Managers.Game.Goods[(Define.GoodsType)Data.upgrade_goods])
        {
            Managers.Game.Goods[(Define.GoodsType)Data.upgrade_goods] -= Data.upgrade_goods_amount;
        }

        Init(Data.upgradeNum);
    }

    private void OnDestroy()
    {
        UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
        uiStatebarGroup.RemoveUnit(this);
    }


    public void OnDead()
    {
        UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
        uiStatebarGroup.SetActive(this, false);
    }

    public void OnDeadTarget()
    {

    }

    public virtual bool ApplyTakeDamage(DamageMessage message)
    {
        Stat.ApplyDamageMessage(ref message);

        return false;
    }

    public virtual IStat GetStat()
    {
        return Stat;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsDead()
    {
        return Stat.IsDead;
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
