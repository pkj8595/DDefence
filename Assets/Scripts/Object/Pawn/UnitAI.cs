using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitAI
{
    private IUnitState _currentState;
    private IdleState _idleState;
    private MoveState _moveState;
    private DeadState _deadState;

    public PawnBase Pawn { get ; private set; }

    public bool HasTarget => Pawn.HasTarget;
    public float SearchRange => Pawn.SearchRange;

    private float _cooltime = 0.15f;
    public void Init(PawnBase pawn)
    {
        Pawn = pawn;
        _idleState = new IdleState(this);
        _moveState = new MoveState(this);
        _deadState = new DeadState(this);

        SetState(_idleState);
    }

    public void OnUpdate()
    {
        if (!Pawn.PawnSkills.IsRunning)
        {
            _currentState.UpdateState();

            _cooltime -= Time.deltaTime;
            if (_cooltime < 0)
            {
                _currentState.AdjustUpdate();
                _cooltime = 0.15f;
            }
        }
    }

    public void SetState(IUnitState newState)
    {
        _currentState?.ExitState();
        _currentState = newState;
        _currentState.EnterState();
    }


    // 상태 객체들
    public IdleState GetIdleState() => _idleState;
    public MoveState GetMoveState() => _moveState;
    public DeadState GetDeadState() => _deadState;


    public void CheckDistenceLockTarget() 
    {
        if (HasTarget)
        {
            if (Vector3.Distance(Pawn.LockTarget.transform.position, Pawn.transform.position) > SearchRange)
            {
                Pawn.LockTarget = null;
            }
        }
    }

}
