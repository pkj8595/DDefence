using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNode : NodeBase, IDamageable
{
    public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    public Vector3 StateBarOffset => throw new System.NotImplementedException();

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

    public virtual Stat GetStat()
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
}
