using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [field: SerializeField] public GameObject PawnObj { get; set; }
    [field: SerializeField] public GameObject ProjectileObj { get; set; }
    [field: SerializeField] public GameObject EffectObj { get; set; }


    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.Game;
        
        
        SelectedManager select = gameObject.GetOrAddComponent<SelectedManager>();
        select.Init();

        var pawns = PawnObj.GetComponentsInChildren<PawnController>();
        for (int i = 0; i < pawns.Length; i++)
        {
            Managers.Game.SetPawnInScene(pawns[i]);
        }

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
            case Define.EParentObj.Effect:
                return EffectObj;
            default:
                Debug.LogError("찾을 수 없는 ParentObj");
                return null;
        }

    }
}
