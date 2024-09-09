using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IUnitState
{
    void Init(UnitAI unitAI);
    void EnterState();
    void UpdateState();
    void AdjustUpdate();
    void ExitState();

    string GetNames();
}

public class IdleState : IUnitState
{
    UnitAI _unitAI;
    PawnBase _pawn;
    public string GetNames() => GetType().Name;

    public void Init(UnitAI unitAI)
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
        _unitAI.TryExecuteSkill();
    }

    public void AdjustUpdate()
    {
        if (!_pawn.HasTarget)
        {
            _pawn.LockTarget = IAttackable.SearchTarget(_pawn, _pawn.SearchRange, _pawn.PawnSkills.GetCurrentSkill().TargetType);
            //적을 발견 했을 경우 상태 전환
            if (_pawn.LockTarget != null)
                _pawn.SetDestination(_pawn.LockTarget.GetTransform().position);
        }
        else
        {
            //타겟이 있을경우
            _unitAI.CheckOutRangeTarget();
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
    public string GetNames() => GetType().Name;


    public void Init(UnitAI unitAI)
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
        //이동 체크
        _unitAI.Pawn.UpdateMove();

        //타겟이 있다면 범위 체크 밖에 나갔다면 
        if (_pawn.HasTarget)
        {
            if (!_unitAI.CheckOutRangeTarget())
                _pawn.TrackingAndAttackTarget();
        }
        else
        {
         //   _unitAI.SetState(_unitAI.GetIdleState());
        }
    }

    public void AdjustUpdate()
    {
        
    }

    public void ExitState()
    {
        _unitAI.Pawn.OnMoveStop();
    }
   
}


public class DeadState : IUnitState
{
    private UnitAI _unitAI;
    public string GetNames() => GetType().Name;

    public void Init(UnitAI unitAI)
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


public class ReturnToBaseState : IUnitState
{
    private UnitAI _unitAI;
    PawnBase _pawn;
    public string GetNames() => GetType().Name;

    public void Init(UnitAI unitAI)
    {
        _unitAI = unitAI;
        _pawn = unitAI.Pawn;
    }

    public void EnterState()
    {
        _pawn.SetDestination(_unitAI.OriginPosition.Value);
    }

    public void UpdateState()
    {
        if (_unitAI.OriginPosition == null)
        {
            _unitAI.SetState(_unitAI.GetIdleState());
        }
    }
    public void AdjustUpdate()
    {
    }

    public void ExitState()
    {

    }


}


public class ChaseState : IUnitState
{
    private UnitAI _unitAI;
    PawnBase _pawn;
    public string GetNames() => GetType().Name;

    public void Init(UnitAI unitAI)
    {
        _unitAI = unitAI;
        _pawn = unitAI.Pawn;
    }

    public void EnterState()
    {
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

public class SkillState : IUnitState
{
    private UnitAI _unitAI;
    PawnBase _pawn;
    public string GetNames() => GetType().Name;

    public void Init(UnitAI unitAI)
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

    }
    public void AdjustUpdate()
    {
    }

    public void ExitState()
    {
    }
}
