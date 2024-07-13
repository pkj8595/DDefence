using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    
    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.Game;
        
        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);


    }

    public override void Clear()
    {
        
    }

    
}
