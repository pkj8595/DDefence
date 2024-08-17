using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public abstract class PawnBase :MonoBehaviour, ISelectedable, IDamageable
{
    private Data.CharacterData _characterData;

    [field: SerializeField] public Define.ETeam Team { get; set; } = Define.ETeam.Playable;
    public Define.WorldObject WorldObjectType { get; set; } = Define.WorldObject.Pawn;
    [SerializeField] protected Define.EPawnAniState _state = Define.EPawnAniState.Idle;
    [SerializeField] protected Vector3 _destPos;
    [SerializeField] protected Transform _projectileTrans;


    ///pawn 기능
    [HideInInspector] public NavMeshAgent _navAgent;
    [field : SerializeField] public PawnAnimationController AniController { get; private set; }
    public PawnStat PawnStat { get; protected set; }
    public UnitSkill PawnSkills { get; } = new UnitSkill();
    public UnitAI AI { get; } = new UnitAI();

    //룬 && 기벽
    [SerializeField] protected List<Data.RuneData> _runeList = new(Define.Pawn_Rune_Limit_Count);
    [SerializeField] private bool _isSelected;

    public float SearchRange { get; set; } = 10f;
    public float LastCombatTime { get; set; } = 0f;
    public Action OnDeadAction { get; set; }

    [field : SerializeField] public IDamageable LockTarget { get; set; }
    public bool HasTarget => LockTarget != null && !LockTarget.IsDead();

    [field: SerializeField] public string AIStateStr { get; set; }

    public virtual Define.EPawnAniState State
    {
        get { return _state; }
        set
        {
            _state = value;
            if (_state == Define.EPawnAniState.Ready || _state == Define.EPawnAniState.Idle)
            {
                if (Time.time < LastCombatTime + 5f)
                    _state = Define.EPawnAniState.Ready;
                else
                    _state = Define.EPawnAniState.Idle;
            }
            
            AniController.SetAniState(_state);
        }
    }

    public Vector3 StateBarOffset => Vector3.up;

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
        AI.OnUpdate();
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
            AI.SetState(AI.GetIdleState());
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
        AI.Init(this);

        Init();
    }

    public void Init(int tableNum, Define.ETeam team)
    {
        Init(tableNum);
        Team = team;
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
    public  bool ApplyTakeDamege(DamageMessage message)
    {
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
            IsExecuteableRange(Vector3.Distance(transform.position, LockTarget.GetTransform().position));
       
        switch (distanceType)
        {
            case Define.ESkillDistanceType.LessMin:
                //방향 벡터를 구해서 target의 반대 방향으로 이동한다.
                Vector3 fleeDirection = transform.position - LockTarget.GetTransform().position;
                SetDestination(fleeDirection.normalized * 
                    (PawnSkills.GetCurrentSkill().MinRange + PawnSkills.GetCurrentSkill().MinRange) * 0.5f);
                break;

            case Define.ESkillDistanceType.Excuteable:
                //스킬이 실행 가능한지 체크 및 자원 소모
                if (PawnSkills.ReadyCurrentSkill(PawnStat))
                {
                    AI.SetState(AI.GetSkillState());
                    transform.LookAt(LockTarget.GetTransform());
                    Skill skill = PawnSkills.GetRunnigSkill();
                    if (skill.MotionDuration > 0)
                    {
                        //StartCoroutine(ExecuteSkill(skill));
                        TaskExecuteSkill(skill).Forget();
                    }
                    else
                    {
                        AniController.SetAniTrigger(skill.AniTriger);
                    }
                }
                break;

            case Define.ESkillDistanceType.MoreMax:
                SetDestination(LockTarget.GetTransform().position);
                break;
        }
    }

    private async UniTaskVoid TaskExecuteSkill(Skill skill)
    {
        State = skill.MotionAni;
        await UniTask.Delay((int)(skill.MotionDuration * 1000));
        if (!IsDead())
            AniController.SetAniTrigger(skill.AniTriger);
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
        if (skill.DetectTargetInSkillRange(this, out List<IDamageable> unitList))
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
                projectile.Init(_projectileTrans, unitList[0].GetTransform(), skill.SplashRange, msg);
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
        AI.SetState(AI.GetIdleState());
    }


    #endregion

    /// <summary>
    /// pawn이 죽었을 때 실행되는 함수
    /// </summary>
    private void OnDead()
    {
        OnDeadAction?.Invoke();
        AI.SetState(AI.GetDeadState());
    }

    private void OnDeadTarget()
    {
        LockTarget = this.SearchTarget(SearchRange, PawnSkills.GetCurrentSkill().TargetType);
        if (LockTarget == null)
        {
            //_ai.SetState(_ai.GetReturnState());
        }
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
        AI.SetState(AI.GetMoveState());
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

    public Transform GetTransform()
    {
        return transform;
    }

    public bool IsDead()
    {
        return PawnStat.IsDead;
    }

    public Stat GetStat()
    {
        return PawnStat;
    }
}
