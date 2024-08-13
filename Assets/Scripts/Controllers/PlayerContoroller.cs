using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerContoroller : PawnBase
{
    //float wait_run_retio = 0.0f;

    int _mask = Define.Layer.Ground.ToInt();

    protected override void Init()
    {
        WorldObjectType = Define.WorldObject.PlayerPawn;
        _pawnStat = gameObject.GetComponent<PawnStat>();

        //Managers.Input.KeyAction -= OnKeyboard;
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

    }

    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position; //방향벡터 생성
        }
    }

    protected override void UpdateIdle()
    {

    }

    protected override void UpdateDie()
    {
    }

    protected override void UpdateMove()
    {
    }
    

    void OnKeyboard()
    {
        
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {

    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        
    }

    //애니메이션 이벤트 키
    public void OnHitEvent()
    {
        Debug.Log("PlayerController : OnHitEvent");
        

    }
}