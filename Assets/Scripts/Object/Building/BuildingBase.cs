using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBase : MonoBehaviour, IDamageable, ISelectedable
{
    public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Building;

    [SerializeField] private Vector3 BuildingStateBarOffset;
    public Vector3 StateBarOffset => BuildingStateBarOffset;

    protected BuildingStat _stat;
    protected Data.BuildingData _data;

    public virtual void Init(int tableNum)
    {
        Init(Managers.Data.BuildingDict[tableNum]);
    }

    public virtual void Init(Data.BuildingData data)
    {
        //todo 건물 테이블 수정하고 구현
        _data = data;
        if (_stat == null)
            _stat = gameObject.GetOrAddComponent<BuildingStat>();
        _stat.Init(data.tableNum, OnDead, OnDeadTarget);
    }


    public void OnDead()
    {

    }
    public void OnDeadTarget()
    {

    }

    public virtual bool ApplyTakeDamage(DamageMessage message)
    {
        _stat.ApplyDamageMessage(ref message);

        return false;
    }

    public virtual IStat GetStat()
    {
        return _stat;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsDead()
    {
        return _stat.IsDead;
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
