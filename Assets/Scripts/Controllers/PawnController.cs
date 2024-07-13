using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : BaseController
{
    public int _testCharacterNum;

    private void Start()
    {
        Init(_testCharacterNum);
    }

    public override void Init(int characterNum)
    {
        base.Init(characterNum);
    }

    protected override void Init()
    {
        base.Init();
    }


    protected override void UpdateDie()
    {
        base.UpdateDie();
    }

    protected override void UpdateIdle()
    {
        base.UpdateIdle();
    }

    protected override void UpdateMove()
    {
        base.UpdateMove();
    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();
    }

    public void OnMove(Vector3 destPosition)
    {
        _destPos = destPosition;
        _destPos.z = 0;
        State = Define.PawnState.Moving;
        _navAgent.SetDestination(_destPos);

    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 rayOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (BoardManager.Instance.GetMoveablePosition(rayOrigin, out Vector2 moveablePosition))
                OnMove(moveablePosition);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            State = Define.PawnState.Attack;
        }
    }
}
