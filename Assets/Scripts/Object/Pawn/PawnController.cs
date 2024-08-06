using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawnController : PawnBase
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

    

    public override void Update()
    {
        base.Update();
     
    }


}
