using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack 
{
    public BaseController _attacker;
    public List<BaseController> _targets = new List<BaseController>();
    public Vector3 _targetPosition;

    public virtual void Execute()
    {
        if (_attacker == null)
        {
            Debug.LogError("공격 실행자가 없음.");
        }
        if (_targets == null && _targets.Count == 0)
        {

        }
    }

    public virtual void SetAttacker(BaseController attacker)
    {
        _attacker = attacker;
    }

    public virtual void SetTarget(BaseController[] targets) 
    {
        _targets.Clear();
        foreach (BaseController b in targets)
        {
            _targets.Add(b);
        }

        if (0 < _targets.Count)
            _targetPosition = _targets[0].gameObject.transform.position;
    }
    
}

public class BasicAttack : Attack
{
    public override void Execute()
    {

    }
}