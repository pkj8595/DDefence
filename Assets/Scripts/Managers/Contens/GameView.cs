using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoSingleton<GameView>
{
    [field : SerializeField] public List<GameObject> GateList { get; set; } = new();

    [field: SerializeField] public GameObject PawnObj { get; set; }
    [field: SerializeField] public GameObject ProjectileObj { get; set; }
    [field: SerializeField] public GameObject EffectObj { get; set; }


}
