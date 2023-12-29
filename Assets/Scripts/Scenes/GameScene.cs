using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    
    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.Game;
        //gameObject.GetOrAddComponent<CursorController>();

        //Dictionary<int, Data.Stat> dic = Managers.Data.StatDict;
        //플레이어 캐릭터 생성
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);

    }

    public override void Clear()
    {
        
    }

    
}
