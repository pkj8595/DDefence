using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    [field: SerializeField] public GameObject PawnGroupObj { get; set; }

    protected override void Init()
    {
        base.Init();
        SCENE_TYPE = Define.Scene.Game;
        
        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);

        AsyncReady().Forget();
    }

    public override void Clear()
    {
        
    }

    async UniTaskVoid AsyncReady()
    {
        PawnGroupObj.SetActive(false);
        await UniTask.WhenAll(BoardManager.Instance.AsyncInit());
        PawnGroupObj.SetActive(true);
    }

}
