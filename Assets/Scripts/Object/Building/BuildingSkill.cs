using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BuildingSkill : MonoBehaviour, IAttackable
{
    public Transform _shotTrans;
    public Animator _animator;
    public IDamageable LockTarget;
    public IDetectComponent DetectComponent;

    protected UnitSkill _skills = new UnitSkill();
    private BuildingBase _buildingBase;
    private bool isInit = false;

    public float SearchRange => _skills.GetCurrentSkill().MaxRange;
    public Define.ETeam Team => _buildingBase.Team;

    public void Init(BuildingBase buildingBase)
    {
        _buildingBase = buildingBase;
        _skills.Init(_buildingBase.Stat.Mana);
        _skills.SetBaseSkill(new Skill( buildingBase.BuildingData.baseSkill));
        DetectComponent = GetComponent<IDetectComponent>();
        isInit = true;
        StartSkillTask().Forget();
    }

    async UniTaskVoid StartSkillTask()
    {
        while (true)
        {
            Skill skill = _skills.GetCurrentSkill();
            if (isInit && skill.IsReady(_buildingBase.Stat.Mana))
            {
                Collider[] colliders = DetectComponent.DetectCollision();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] == null)
                        continue;

                    if (colliders[i].TryGetComponent(out IDamageable damageable))
                    {
                        if (SearchTargetToRay(damageable, skill))
                        {
                            StartSkill();
                        }
                    }
                }
            }

            await UniTask.Delay(200, cancellationToken: gameObject.GetCancellationTokenOnDestroy());
        }
    }

    private void StartSkill()
    {
        if(_animator != null)
        {
            _animator.Play("Attack");
        }
        else
        {
            if (_skills.ReadyCurrentSkill(_buildingBase.Stat))
            {
                Skill skill = _skills.GetRunnigSkill();
                DamageMessage msg = new DamageMessage(_buildingBase.Stat,
                                                      Vector3.zero,
                                                      Vector3.zero,
                                                      skill);

                ProjectileBase projectileBase = skill.MakeProjectile();
                projectileBase.Init(_shotTrans.transform,
                                    LockTarget.GetTransform(),
                                    _skills.GetRunnigSkill().SplashRange,
                                    msg);
                EndSkill();
            }
        }
    }

    /// <summary>
    /// animation callback 
    /// </summary>
    public void BegineSkill()
    {
        if (_skills.ReadyCurrentSkill(_buildingBase.Stat))
        {
            Skill skill = _skills.GetRunnigSkill();
            DamageMessage msg = new DamageMessage(_buildingBase.Stat,
                                                  Vector3.zero,
                                                  Vector3.zero,
                                                  skill);

            LockTarget.ApplyTakeDamage(msg);
        }
    }

    /// <summary>
    /// animation callback
    /// </summary>
    public void EndSkill()
    {
        _buildingBase.Stat.IncreadMana();
        _skills.ClearCurrentSkill();
    }

    /// <summary>
    /// 스킬에 맞는 타겟을 찾아서 ray에 맞고 그 개체가 죽지 않았다면 true
    /// </summary>
    /// <param name="other"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    private bool SearchTargetToRay(IDamageable other, Skill skill)
    {
        if (this.GetTargetType(other.Team) != skill.TargetType)
        {
            return false;
        }

        float skillRange = skill.MaxRange;
        int layer = (int)Define.Layer.Pawn;
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
