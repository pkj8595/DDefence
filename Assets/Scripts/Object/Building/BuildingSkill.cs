using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSkill : MonoBehaviour
{
    public Transform _shotTrans;
    public Collider _detectCollider;

    BuildingBase _buildingBase;
    protected UnitSkill _skills = new UnitSkill();

    public IDamageable LockTarget;
    public HashSet<IDamageable> _pawnListInRange = new ();

    public void Init(BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _skills.Init(_buildingBase.Stat.Mana);
        _skills.SetBaseSkill(new Skill( buildingBase.Data.baseSkill));
        _detectCollider.isTrigger = true;
    }

    public void Update()
    {
        if (0 < _pawnListInRange.Count)
        {
            Skill skill = _skills.GetCurrentSkill();
            if (!skill.IsReady(_buildingBase.Stat.Mana))
                return;

            foreach (IDamageable target in _pawnListInRange)
            {
                if (SearchTarget(target, skill))
                {
                    StartSkill();
                }
            }
        }
    }

    private void StartSkill()
    {
        if (_skills.ReadyCurrentSkill(_buildingBase.GetStat()))
        {
            Skill skill = _skills.GetRunnigSkill();
            DamageMessage msg = new DamageMessage(_buildingBase.Stat,
                                                  Vector3.zero,
                                                  Vector3.zero,
                                                  skill);

            ProjectileBase projectileBase = skill.MakeProjectile(
                Managers.Scene.CurrentScene.GetParentObj(Define.EParentObj.Projectile).transform
                );

            projectileBase.Init(_shotTrans.transform, 
                                LockTarget.GetTransform(),
                                _skills.GetRunnigSkill().SplashRange,
                                msg);
            _buildingBase.Stat.IncreadMana();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPawn(other))
            return;

        _pawnListInRange.Add(other.attachedRigidbody.GetComponent<IDamageable>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPawn(other))
            return;

        _pawnListInRange.Remove(other.attachedRigidbody.GetComponent<IDamageable>());
    }

    private bool IsPawn(Collider other)
    {
        int layer = (int)Define.Layer.Pawn;
        if (1 << other.gameObject.layer != layer)
            return false;

        return true;
    }

    /// <summary>
    /// 스킬에 맞는 타겟을 찾아서 ray에 맞고 그 개체가 죽지 않았다면 true
    /// </summary>
    /// <param name="other"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    private bool SearchTarget(IDamageable other, Skill skill)
    {
        if (_buildingBase.GetTargetType(other.Team) != skill.TargetType)
        {
            return false;
        }

        float skillRange = skill.MaxRange;
        int layer = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
        Vector3 directionEnumy = other.GetTransform().position - _shotTrans.position;
        if (Physics.Raycast(_shotTrans.position, directionEnumy, out RaycastHit hit, skillRange, layer)) 
        {
            if (hit.transform == other.GetTransform())
            {
                if (!other.IsDead())
                {
                    LockTarget = other;
                    return true;
                }
            }
        }
        return false;
    }



}
