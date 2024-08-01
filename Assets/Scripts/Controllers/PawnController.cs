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
        if (_lockTarget)
        {

        }
        else
        {

        }

    }

    protected override void UpdateMove()
    {
        base.UpdateMove();

    }

    protected override void UpdateSkill()
    {
        base.UpdateSkill();
    }

    protected override void OnLateUpdate()
    {
        base.OnLateUpdate();

    }

    public void OnMove(Vector3 destPosition)
    {
        _destPos = destPosition;
        State = Define.EPawnAniState.Moving;
        _navAgent.SetDestination(_destPos);

    }

    public override void Update()
    {
        base.Update();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Utils.GetMouseWorldPositionToRay((position) => {
        //        if (BoardManager.Instance.GetMoveablePosition(position, out Vector3 moveablePosition))
        //        {
        //            OnMove(moveablePosition);
        //        }
        //    });
        //}

     

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



}
