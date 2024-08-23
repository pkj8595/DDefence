using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public Define.ETeam Team { get; set; }
    public Define.WorldObject WorldObjectType { get; set; }
    public Vector3 StateBarOffset { get; }

    public abstract bool ApplyTakeDamage(DamageMessage message);
    public abstract Transform GetTransform();
    public abstract bool IsDead();
    public IStat GetStat();


    
}

public interface IAttackable
{
    public float SearchRange { get;}

    /// <summary>
    /// 적 
    /// </summary>
    /// <param name="searchRange"></param>
    /// <returns></returns>
    public static IDamageable SearchTarget(IDamageable transform, float searchRange, Define.ETargetType targetType)
    {
        if (Define.ETargetType.Self == targetType)
            return transform;

        int layerTarget = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
        Collider[] colliders = Physics.OverlapSphere(transform.GetTransform().position, searchRange, layerTarget);

        foreach (var collider in colliders)
        {
            IDamageable unit = collider.attachedRigidbody.GetComponent<IDamageable>();
            if (unit != null && !unit.IsDead() && unit.GetTargetType(transform.Team) == targetType)
            {
                return unit;
            }
        }
        return null;
    }
}

