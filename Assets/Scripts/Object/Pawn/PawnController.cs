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

    public override void Update()
    {
        base.Update();
     
    }


}
