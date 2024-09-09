using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAI
{
    private IUnitState                  _currentState;
    private readonly IdleState          _idleState = new IdleState();
    private readonly MoveState          _moveState = new MoveState();
    private readonly DeadState          _deadState = new DeadState();
    private readonly SkillState         _skillState = new SkillState();
    private readonly ReturnToBaseState  _returnState = new ReturnToBaseState();

    public PawnBase Pawn { get; private set; }
    public bool HasTarget => Pawn.HasTarget;
    public float SearchRange => Pawn.SearchRange;
    public Vector3? OriginPosition { get; set; }

    private float _cooltime = 0.0f;
    public void Init(PawnBase pawn)
    {
        Pawn = pawn;

        _idleState.Init(this);
        _moveState.Init(this);
        _deadState.Init(this);
        _skillState.Init(this);
        _returnState.Init(this);

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
    /// 타겟이 범위 밖으로 나갔다면 target추적 중지
    /// </summary>
    public bool CheckOutRangeTarget() 
    {
        if (SearchRange < Vector3.Distance(Pawn.LockTarget.GetTransform().position, Pawn.transform.position) && Pawn.Team == Define.ETeam.Playable)
        {
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
