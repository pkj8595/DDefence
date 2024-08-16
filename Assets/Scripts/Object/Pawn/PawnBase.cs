using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PawnBase : Unit, ISelectedable
{
    private Data.CharacterData _characterData;

    [SerializeField] protected Define.EPawnAniState _state = Define.EPawnAniState.Idle;
    [HideInInspector] public NavMeshAgent _navAgent;
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected Transform _projectileTrans;

    [field : SerializeField] public Unit LockTarget { get; set; }

    [SerializeField] protected List<Data.RuneData> _runeList = new(Define.Pawn_Rune_Limit_Count);
    [SerializeField] private bool _isSelected;

    ///pawn 기능
    [field : SerializeField] public PawnAnimationController AniController { get; private set; }
    public PawnStat PawnStat { get; protected set; }
    public UnitSkill PawnSkills { get; } = new UnitSkill();
    private UnitAI _ai = new UnitAI();


    public Action OnDeadAction { get; set; }
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Unknown;
    public float SearchRange { get; set; } = 10f;
    public bool HasTarget => LockTarget != null && !LockTarget.IsDead();
    public float LastCombatTime { get; set; } = 0f;
    [field: SerializeField] public string AIStateStr { get; set; }

    public virtual Define.EPawnAniState State
    {
        get { return _state; }
        set
        {
            if (value == Define.EPawnAniState.Ready || value == Define.EPawnAniState.Idle)
            {
                if (Time.time < LastCombatTime + 5f)
                    value = Define.EPawnAniState.Ready;
                else
                    value = Define.EPawnAniState.Idle;
            }
            _state = value;
            AniController.SetAniState(_state);
        }
    }

    public virtual void SetTriggerAni(Define.EPawnAniTriger trigger)
    {
        AniController.SetAniTrigger(trigger);
    }


    private void Awake()
    {
        if (AniController == null)
            AniController = gameObject.GetComponentInChildren<PawnAnimationController>();

        if (PawnStat == null)
            PawnStat = gameObject.GetOrAddComponent<PawnStat>();

        if (_navAgent == null)
            _navAgent.GetComponent<NavMeshAgent>();
        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;

    }


    public virtual void Update()
    {
        _ai.OnUpdate();
    }

    public void UpdateMove()
    {
        switch (_navAgent.pathStatus)
        {
            case NavMeshPathStatus.PathComplete:
                //Debug.Log("경로가 완전히 생성.");
                break;
            case NavMeshPathStatus.PathPartial:
                //Debug.Log("일부만 생성.");
                if (_navAgent.path.corners != null)
                    _destPos = _navAgent.path.corners[_navAgent.path.corners.Length - 1];
                break;
            case NavMeshPathStatus.PathInvalid:
                //Debug.Log("유효하지 않는 경로");
                _navAgent.isStopped = true;
                _navAgent.ResetPath();
                State = Define.EPawnAniState.Idle;
                return;
        }

        //naviAgent가 이동을 마쳤을 경우 idle로 돌아감
        if (_navAgent.velocity == Vector3.zero && Vector3.Distance(_destPos, transform.position) < 0.2f)
        {
            //State = _lockTarget == null ? Define.EPawnAniState.Idle : Define.EPawnAniState.Ready;
            _ai.SetState(_ai.GetIdleState());
        }


        if (_navAgent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_navAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }
    }

    public virtual void Init(int characterNum)
    {
        //table data setting
        _characterData = Managers.Data.CharacterDict[characterNum];
        AniController.Init(_characterData);
        PawnStat.Init(_characterData.statDataNum, OnDead, OnDeadTarget);

        //component setting

        //_colliderAttackRange.radius = _stat.AttackRange;
        _navAgent.speed = PawnStat.MoveSpeed;
        PawnSkills.Init(PawnStat.Mana);
        PawnSkills.SetBaseSkill(new Skill(_characterData.basicSkill));
        _ai.Init(this);

        Init();
    }

    protected virtual void Init() { }


#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward);
    }

#endif

    #region combat system
    /// <summary>
    /// 피격시 실행되는 함수 전달받은 매개변수로 캐릭터 데이터 갱신
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public override bool ApplyTakeDamege(DamageMessage message)
    {
        //todo : 스탯과 스킬데이터로 정보 갱신
        SetTriggerAni(Define.EPawnAniTriger.Hit);
        PawnStat.ApplyDamageMessage(ref message);
        LastCombatTime = Time.time;
        return false;
    }


    /// <summary>
    /// 추척 대상이 있을때 이동 및 공격
    /// </summary>
    public void TrackingAndAttackTarget()
    {
        Define.ESkillDistanceType distanceType = PawnSkills.GetCurrentSkill().
            IsExecuteableRange(Vector3.Distance(transform.position, LockTarget.transform.position));

       
        switch (distanceType)
        {
            case Define.ESkillDistanceType.LessMin:
                //방향 벡터를 구해서 target의 반대 방향으로 이동한다.
                Vector3 fleeDirection = transform.position - LockTarget.transform.position;
                SetDestination(fleeDirection.normalized * 
                    (PawnSkills.GetCurrentSkill().MinRange + PawnSkills.GetCurrentSkill().MinRange) * 0.5f);
                break;

            case Define.ESkillDistanceType.Excuteable:
                //스킬이 실행 가능한지 체크 및 자원 소모
                if (PawnSkills.ReadyCurrentSkill(PawnStat))
                {
                    _ai.SetState(_ai.GetSkillState());
                    transform.LookAt(LockTarget.transform);
                    Skill skill = PawnSkills.GetRunnigSkill();
                    if (skill.MotionDuration > 0)
                    {
                        StartCoroutine(ExecuteSkill(skill));
                    }
                    else
                    {
                        AniController.SetAniTrigger(skill.AniTriger);
                    }
                }
                break;

            case Define.ESkillDistanceType.MoreMax:
                SetDestination(LockTarget.transform.position);
                break;
        }
    }

    IEnumerator ExecuteSkill(Skill skill)
    {
        State = skill.MotionAni;
        yield return YieldCache.WaitForSeconds(skill.MotionDuration);
        AniController.SetAniTrigger(skill.AniTriger);
    }

    /// <summary>
    /// 타격타이밍에 실행 -> animation에서 OnHitEvent 호출시 
    /// </summary>
    public virtual void BegineAniAttack()
    {
        //projectile 발사
        Skill skill = PawnSkills.GetRunnigSkill();
        DamageMessage msg = new DamageMessage(PawnStat,
                                           Vector3.zero,
                                           Vector3.zero,
                                           skill.AffectList.ToArray());
        if (skill.DetectTargetInSkillRange(this, out List<Unit> unitList))
        {
            ProjectileBase projectile = skill.MakeProjectile(
                Managers.Scene.CurrentScene.GetParentObj(Define.EParentObj.Projectile).transform
                );

            if (projectile == null)
            {
                for (int i = 0; i < unitList.Count; i++)
                {
                    unitList[i].ApplyTakeDamege(msg);
                }
            }
            else
            {
                projectile.Init(_projectileTrans, unitList[0].transform, skill.SplashRange, msg);
            }
        }
    }

    /// <summary>
    ///  Animation Event 함수 실행시
    /// </summary>
    public virtual void EndAniAttack()
    {
        PawnSkills.ClearCurrentSkill();
        LastCombatTime = Time.time;
        PawnStat.IncreadMana();
        _ai.SetState(_ai.GetIdleState());
    }


    #endregion

    /// <summary>
    /// pawn이 죽었을 때 실행되는 함수
    /// </summary>
    private void OnDead()
    {
        OnDeadAction?.Invoke();
        _ai.SetState(_ai.GetDeadState());
    }

    private void OnDeadTarget()
    {
        LockTarget = SearchTarget(SearchRange, PawnSkills.GetCurrentSkill().TargetType);
        if (LockTarget == null)
        {
            //_ai.SetState(_ai.GetReturnState());
        }
    }

    public override bool IsDead()
    {
        return PawnStat.IsDead;
    }

    public void SetDestination(Vector3 position)
    {
        if (_destPos == position)
            return;
        if (BoardManager.Instance.GetMoveablePosition(position, out Vector3 moveablePosition))
        {
            OnMove(moveablePosition);
        }
    }

    protected virtual void OnMove(Vector3 destPosition)
    {
        if (_destPos == destPosition)
            return;
        _destPos = destPosition;
        _navAgent.SetDestination(_destPos);
        _ai.SetState(_ai.GetMoveState());
    }

    /// <summary>
    /// 길찾기 종료
    /// </summary>
    public void OnMoveStop()
    {
        _destPos = gameObject.transform.position;
        _destPos.z = 0;
        _navAgent.ResetPath();
    }

    public virtual void OnSelect()
    {
        _isSelected = true;
    }

    public virtual void OnDeSelect()
    {
        _isSelected = false;
    }

    public virtual bool IsSelected()
    {
        return _isSelected;
    }


}
