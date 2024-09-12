using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAI
{
    private IUnitState                  _currentState;
    private readonly IdleState          _idleState = new IdleState();
    private readonly ChaseState         _chaseState = new ChaseState();
    private readonly DeadState          _deadState = new DeadState();
    private readonly SkillState         _skillState = new SkillState();
    private readonly MoveState         _moveState = new MoveState();

    public PawnBase Pawn { get; private set; }
    public bool HasTarget => Pawn.HasTarget;
    public float SearchRange => Pawn.SearchRange;
    public Vector3? OriginPosition { get; set; }

    private float _cooltime = 0.0f;
    public void Init(PawnBase pawn)
    {
        Pawn = pawn;

        _idleState.Init(this);
        _chaseState.Init(this);
        _deadState.Init(this);
        _skillState.Init(this);
        _moveState.Init(this);

        SetState(_idleState);
    }

    public void OnUpdate()
    {
        _cooltime -= Time.deltaTime;
        if (_cooltime < 0)
        {
            _currentState.AdjustUpdate();
            _cooltime += 0.3f;
        }
        else
        {
            _currentState.UpdateState();
        }
    }

    public void SetState(IUnitState newState)
    {
        if (_currentState != newState)
        {
            Pawn.AIStateStr = newState.GetNames();
            _currentState?.ExitState();
            _currentState = newState;
        }
        _currentState.EnterState();
    }

    // 상태 객체들
    public IdleState GetIdleState() => _idleState;
    public ChaseState GetChaseState() => _chaseState;
    public DeadState GetDeadState() => _deadState;
    public SkillState GetSkillState() => _skillState;
    public MoveState GetMoveState() => _moveState;

    /// <summary>
    /// 타겟이 범위 밖으로 나갔다면 target추적 중지
    /// </summary>
    public bool CheckOutRangeTarget() 
    {
        if (SearchRange + 3 < Vector3.Distance(Pawn.LockTarget.GetTransform().position, Pawn.transform.position) && Pawn.Team == Define.ETeam.Playable)
        {
            SetState(GetIdleState());
            return true;
        }
        return false;
    }

    public void TrackingAndAttackTarget()
    {
        if (!Pawn.IsDead() && Pawn.HasTarget)
        {
            Pawn.TrackingAndAttackTarget();
        }
    }

    public void SearchTarget()
    {
        Pawn.LockTarget = IAttackable.SearchTarget(Pawn, Pawn.SearchRange, Pawn.PawnSkills.GetCurrentSkill().TargetType);
        //적을 발견 했을 경우 상태 전환
        if (Pawn.LockTarget != null)
            Pawn.SetDestination(Pawn.LockTarget.GetTransform().position);
    }

}
