using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : Unit, ISelectedable, IWaveEvent
{
    [SerializeField] public GameObject _model;
    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    public Data.BuildingData BuildingData { get; private set; }
    protected BuildingProduction _production;
    protected BuildingDamageable _damageable;
    public  BuildingStat Stat { get; private set; }
    public BuildingSkill Skill { get; protected set; }

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
            Skill = gameObject.GetOrAddComponent<BuildingSkill>();
            Skill.Init(this);
        }

        if (data.isDamageable)
        {
            _damageable = gameObject.GetOrAddComponent<BuildingDamageable>();
            _damageable.Init(this);

            UIStateBarGroup uiStatebarGroup = Managers.UI.ShowUI<UIStateBarGroup>() as UIStateBarGroup;
            uiStatebarGroup.AddUnit(_damageable);
        }

        
    }

    public override bool UpgradeUnit()
    {
        if (BuildingData.upgradeNum == 0 || Team == Define.ETeam.Enemy)
        {
            Managers.UI.ShowToastMessage("적은 업그레이드 할 수 없습니다.");
            return false;
        }

        if (Managers.Game.Inven.SpendItem(BuildingData.upgrade_goods, BuildingData.upgrade_goods_amount))
        {
            Init(BuildingData.upgradeNum);
            return true;
        }

        Managers.UI.ShowToastMessage("업그레이드 비용이 부족합니다.");
        return false;
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
        UIData data = new UIUnitData { unitGameObject = this };
        Managers.UI.ShowUIPopup<UIUnitPopup>(data);
    }

    public void OnDeSelect()
    {
    }

    public bool IsSelected()
    {
        return false;
    }

    public void EndWave()
    {
        Managers.Game.Inven.SpendWaveCost(Define.EGoodsType.gold, BuildingData.waveCost);
    }

    public void ReadyWave()
    {
        
    }
    #endregion

}
