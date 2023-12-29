using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TilePrefabBase
{
    public SpriteRenderer _renderer;
    public GameObject _target;
    public CircleCollider2D _attackRangeCollider;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Init()
    {
        base.Init();

    }

    public override void Clear()
    {
        base.Clear();

    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void StartAnimation_Attack()
    {

    }

    private void StartAnimation_Rotate()
    {

    }

}
