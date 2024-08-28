using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameScene : BaseScene
{
    [field: SerializeField] public GameObject PawnObj { get; set; }

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

        StartGame().Forget();
    }

    public override void Clear()
    {
        
    }

    private void Update()
    {
        
    }

    async UniTaskVoid StartGame()
    {
        await UniTask.Delay(2000);
        Managers.Game.StartGame();
    }
}
