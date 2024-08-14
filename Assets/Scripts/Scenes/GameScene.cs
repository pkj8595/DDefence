using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [field: SerializeField] public PawnGroup SelectdPawnGroup { get; set; }
    [field: SerializeField] public GameObject PawnObj { get; set; }
    [field: SerializeField] public GameObject ProjectileObj { get; set; }


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

    private void Update()
    {
        
    }

    public override GameObject GetParentObj(Define.EParentObj obj)
    {
        switch (obj)
        {
            case Define.EParentObj.Pawn:
                return PawnObj;
            case Define.EParentObj.Projectile:
                return ProjectileObj;
            default:
                Debug.LogError("찾을 수 없는 ParentObj");
                return null;
        }
        
    }
}
