using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력바가 있는 모든 오브젝트에 사용
/// </summary>
public abstract class Unit : MonoBehaviour, IDamageable
{
    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;

    public abstract bool IsDead();

    /// <summary>
    /// 팀이 다를경우 타겟 타입을 Enemy로 반환
    /// </summary>
    /// <param name="pawnTeam"></param>
    /// <returns></returns>
    public Define.ETargetType GetTargetType(Define.ETeam pawnTeam)
    {
        if (Team == pawnTeam)
            return Define.ETargetType.Ally;
        else
            return Define.ETargetType.Enemy;
    }

    public virtual Unit SearchTarget(float searchRange)
    {
        int layerTarget = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRange, layerTarget);

        foreach (var collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null && !unit.IsDead() && unit.GetTargetType(this.Team) == Define.ETargetType.Enemy)
            {
                return unit;
            }
        }
        return null;
    }

    public virtual bool ApplyTakeDamege(DamageMessage message)
    {
        return false;
    }
}
