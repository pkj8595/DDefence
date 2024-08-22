using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAI
{
    private IUnitState _currentState;
    private IdleState _idleState;
    private MoveState _moveState;
    private DeadState _deadState;
    private SkillState _skillState;
    private ReturnToBaseState _returnState;

    public PawnBase Pawn { get; private set; }
    public bool HasTarget => Pawn.HasTarget;
    public float SearchRange => Pawn.SearchRange;
    public Vector3 OriginPosition { get; set; }

    private float _cooltime = 0.0f;
    public void Init(PawnBase pawn)
    {
        Pawn = pawn;
        _idleState = new IdleState(this);
        _moveState = new MoveState(this);
        _deadState = new DeadState(this);
        _skillState = new SkillState(this);
        _returnState = new ReturnToBaseState(this);

        SetState(_idleState);
    }

    public void OnUpdate()
    {
        _cooltime -= Time.deltaTime;
        if (_cooltime < 0)
        {
            _currentState.AdjustUpdate();
            _cooltime += 0.2f;
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
    public MoveState GetMoveState() => _moveState;
    public DeadState GetDeadState() => _deadState;
    public SkillState GetSkillState() => _skillState;
    public ReturnToBaseState GetReturnPosState() => _returnState;

    /// <summary>
    /// 타겟이 범위 밖으로 나갔다면 target추적 중지 및 재 탐색
    /// </summary>
    public bool CheckOutRangeTarget() 
    {
        if (SearchRange < Vector3.Distance(Pawn.LockTarget.GetTransform().position, Pawn.transform.position))
        {
            Pawn.LockTarget = IAttackable.SearchTarget(Pawn, SearchRange, Pawn.PawnSkills.GetCurrentSkill().TargetType);
            SetState(GetIdleState());
            return true;
        }
        return false;
    }

    public void TryExecuteSkill()
    {
        if (Pawn.HasTarget)
        {
            Skill skill = Pawn.PawnSkills.GetCurrentSkill();
            if (skill.IsReady(Pawn.PawnStat.Mana))
            {
                Pawn.TrackingAndAttackTarget();
            }
        }
    }

}
