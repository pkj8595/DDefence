using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PawnBase : Unit, ISelectedable
{
    private Data.CharacterData _characterData;

    [SerializeField] protected Define.EPawnAniState _state = Define.EPawnAniState.Idle;
    [SerializeField] protected PawnAnimationController _aniController;
    [SerializeField] public NavMeshAgent _navAgent;

    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected Unit _lockTarget;
    [SerializeField] protected PawnStat _pawnStat;
    [SerializeField] private bool _isSelected;
    [SerializeField] protected UnitSkill _unitSkill = new UnitSkill();

    [SerializeField] protected List<Data.RuneData> _runeList = new(Define.Pawn_Rune_Limit_Count);

    public Action OnDeadAction { get; set; }
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Unknown;
    public float SearchRange { get; set; } = 10f;
    public bool HasTarget => _lockTarget != null && !_lockTarget.IsDead();

    public virtual Define.EPawnAniState State
    {
        get { return _state; }
        set
        {
            _state = value;
            _aniController.SetAniState(_state);
        }
    }

    public virtual void SetTriggerAni(Define.EPawnAniTriger trigger)
    {
        _aniController.SetAniTrigger(trigger);
    }


    private void Awake()
    {
        if (_aniController == null)
            _aniController = gameObject.GetOrAddComponent<PawnAnimationController>();

        if (_pawnStat == null)
            _pawnStat = gameObject.GetOrAddComponent<PawnStat>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
        
    }


    public virtual void Update()
    {
        switch (State)
        {
            case Define.EPawnAniState.Dead:
                UpdateDie();
                break;
            case Define.EPawnAniState.Running:
                UpdateMove();
                break;
            case Define.EPawnAniState.Idle:
                UpdateIdle();
                break;
            case Define.EPawnAniState.Ready:
                UpdateReady();
                break;
        }

    }

    public void LateUpdate()
    {
        OnLateUpdate();
    }

    public virtual void Init(int characterNum)
    {
        //table data setting
        _characterData = Managers.Data.CharacterDict[characterNum];
        _aniController.Init(_characterData);
        _pawnStat.Init(_characterData.statDataNum, OnDead);

        //component setting

        //_colliderAttackRange.radius = _stat.AttackRange;
        _navAgent.speed = _pawnStat.MoveSpeed;
        _unitSkill.Init(_pawnStat.Mana);
        _unitSkill.SetBaseSkill(new Skill(_characterData.basicSkill));

        Init();
    }

    protected virtual void Init() { }

    #region update
    protected virtual void UpdateDie() 
    {
        //OnDeadAction?.Invoke();
    }

    protected virtual void UpdateMove() 
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
        if (_navAgent.velocity == Vector3.zero && Vector3.Distance(_destPos, transform.position) < 0.1f)
        {
            State = _lockTarget == null ? Define.EPawnAniState.Idle : Define.EPawnAniState.Ready;
        }


        if (_navAgent.velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_navAgent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1);
        }
    }

    protected virtual void OnLateUpdate() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateReady() { }
    #endregion


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
        _pawnStat.ApplyDamageMessage(ref message);
        return false;
    }

    //todo 코루틴을 쓰든 update를 쓰든 바꾸기
    IEnumerator UpdatePath()
    {
        while (!this.IsDead())
        {
            if (!_unitSkill.IsRunning)
            {
                if (HasTarget)
                {
                    if (State == Define.EPawnAniState.Idle || State == Define.EPawnAniState.Ready)
                    {
                        //트랙킹애니메이션으로 
                    }

                    Define.ESkillDistanceType distanceType = _unitSkill.GetCurrentSkill().
                        IsExcuteableRange(Vector3.Distance(transform.position, _lockTarget.transform.position));

                    switch (distanceType)
                    {
                        case Define.ESkillDistanceType.LessMin:
                            Vector3 fleeDirection = _lockTarget.transform.position - transform.position;
                            fleeDirection = -fleeDirection.normalized * 2;
                            SetDestination(fleeDirection);
                            break;
                        case Define.ESkillDistanceType.Excuteable:
                            //스킬이 실행 가능한지 체크 및 자원 소모
                            if (_unitSkill.ReadyCurrentSkill(_pawnStat))
                            {
                                StartCoroutine(ExecuteSkill(_unitSkill.GetRunnigSkill()));
                            }
                            break;
                        case Define.ESkillDistanceType.MoreMax:
                            SetDestination(_lockTarget.transform.position);
                            break;
                    }
                }
                else
                {
                    _lockTarget = SearchTarget(SearchRange);
                }
            }

            yield return YieldCache.WaitForSeconds(0.5f);
        }
    }

    IEnumerator ExecuteSkill(Skill skill)
    {
        if (skill.MotionDuration > 0)
        {
            State = skill.MotionAni;
            yield return YieldCache.WaitForSeconds(skill.MotionDuration);
        }

        _aniController.SetAniTrigger(skill.AniTriger);
    }

    /// <summary>
    /// 타격타이밍에 실행 -> animation에서 OnHitEvent 호출시 
    /// </summary>
    public virtual void BegineAniAttack()
    {
        
    }

    /// <summary>
    ///  Animation Event 함수 실행시
    /// </summary>
    public virtual void EndAniAttack()
    {
        //projectile 발사
        Skill skill = _unitSkill.GetRunnigSkill();
        DamageMessage msg = new DamageMessage(_pawnStat,
                                           Vector3.zero,
                                           Vector3.zero,
                                           skill.AffectList.ToArray());
        List<Unit> unitList;
        if (skill.DetectTargetInSkillRange(this, out unitList))
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
                projectile.Init(unitList[0].transform, skill.SplashRange, msg);
            }
        }
        _unitSkill.ClearCurrentSkill();
    }

    public virtual void ReadyShot()
    {
        //적 감지후 거리거리 내에 있을때
        //shot포인트에 캐릭터에 맞는 projectile 스폰
        //target, skill정보, damageMessage 생성
    }
    
    #endregion

    /// <summary>
    /// pawn이 죽었을 때 실행하는 함수
    /// </summary>
    public void OnDead()
    {
        OnDeadAction?.Invoke();
        State = Define.EPawnAniState.Dead;
    }

    public override bool IsDead()
    {
        return _pawnStat.IsDead;
    }

    public void SetDestination(Vector3 position)
    {
        if (BoardManager.Instance.GetMoveablePosition(position, out Vector3 moveablePosition))
        {
            OnMove(moveablePosition);
        }
    }

    protected virtual void OnMove(Vector3 destPosition)
    {
        _destPos = destPosition;
        State = Define.EPawnAniState.Running;
        _navAgent.SetDestination(_destPos);
    }

    /// <summary>
    /// 길찾기 종료
    /// </summary>
    public void OnStop()
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
