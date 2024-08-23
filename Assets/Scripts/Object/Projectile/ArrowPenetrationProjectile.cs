using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowPenetrationProjectile : ProjectileBase
{
    public Vector3 _offsetTarget;
    public float arrowRange = 10f;
    public float duration = 1f;
    Sequence sequence;

    public override void Init(Transform startTrans, Transform target, float splashRange, in DamageMessage msg)
    {
        base.Init(startTrans, target, splashRange, msg);
        Shot();
    }

    public void Shot()
    {
        if (_target == null) return;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(transform.forward * _msg.skill.MaxRange, duration)
                .SetEase(Ease.Linear));
        sequence.AppendCallback(() => { Destroy(); });
    }

    protected override void Destroy()
    {
        gameObject.SetActive(false);
        Managers.Resource.Destroy(gameObject);
    }

    protected override void HandleImpact(Collider[] others)
    {
        base.HandleImpact(others);
        
        _projectileCollider.enabled = false;
        sequence.Kill();
        Invoke(nameof(Destroy), 2f);
    }

}
