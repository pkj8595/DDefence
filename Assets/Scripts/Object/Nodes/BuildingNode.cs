using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNode : NodeBase, IDamageable, ISelectedable
{
    public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    [SerializeField] private Vector3 BuildingStateBarOffset;
    public Vector3 StateBarOffset => BuildingStateBarOffset;

    private BuildingStat _stat;

    public virtual void InitBuildingNode(int tableNum)
    {
        InitBuildingNode(Managers.Data.BuildingDict[tableNum]);
    }

    public virtual void InitBuildingNode(Data.BuildingData data)
    {
        //todo 건물 테이블 수정하고 구현
    }


    public virtual bool ApplyTakeDamege(DamageMessage message)
    {
        return false;
    }

    public virtual IStat GetStat()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsDead()
    {
        throw new System.NotImplementedException();
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
