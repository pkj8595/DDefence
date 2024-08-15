using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IUnitState
{
    void EnterState();
    void UpdateState();
    void AdjustUpdate();
    void ExitState();
}

public class IdleState : IUnitState
{
    UnitAI _unitAI;
    PawnBase _pawn;

    public IdleState(UnitAI unitAI)
    {
        _unitAI = unitAI;
        _pawn = unitAI.Pawn;
    }

    public void EnterState()
    {
        _unitAI.Pawn.State = Define.EPawnAniState.Idle;
    }
    public void UpdateState()
    {
        if (_pawn.HasTarget)
        {
            Skill skill = _pawn.PawnSkills.GetCurrentSkill();
            if (skill.IsReady(_pawn.PawnStat.Mana))
            {
                _pawn.TrackingTarget();
            }
        }
    }

    public void AdjustUpdate()
    {
        if (!_pawn.HasTarget)
        {
            _pawn.LockTarget = _pawn.SearchTarget(_pawn.SearchRange, _pawn.PawnSkills.GetCurrentSkill().TargetType);
            //적을 발견 했을 경우 상태 전환
            if (_pawn.LockTarget != null)
                _unitAI.SetState(_unitAI.GetMoveState());
        }
        else
        {
            //타겟이 있을경우
            _unitAI.CheckDistenceLockTarget();
        }
        
    }

    public void ExitState()
    {
        
    }
}

public class MoveState : IUnitState
{
    private UnitAI _unitAI;
    PawnBase _pawn;


    public MoveState(UnitAI unitAI)
    {
        _unitAI = unitAI;
        _pawn = unitAI.Pawn;
    }

    public void EnterState()
    {
        _pawn.State = Define.EPawnAniState.Running;

    }

    public void UpdateState()
    {
        _unitAI.Pawn.UpdateMove();
        _unitAI.CheckDistenceLockTarget();
    }

    public void AdjustUpdate()
    {
        if (_pawn.HasTarget)
        {
            _pawn.TrackingTarget();
        }
    }

    public void ExitState()
    {
        _unitAI.Pawn.OnMoveStop();
    }

}


public class DeadState : IUnitState
{
    private UnitAI _unitAI;

    public DeadState(UnitAI unitAI)
    {
        _unitAI = unitAI;
    }

    public void EnterState()
    {
        _unitAI.Pawn.State = Define.EPawnAniState.Dead;
    }

    public void UpdateState()
    {

    }
    public void AdjustUpdate()
    {
    }

    public void ExitState()
    {
    }

   
}
