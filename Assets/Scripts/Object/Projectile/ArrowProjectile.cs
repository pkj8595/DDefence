using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowProjectile : ProjectileBase
{
    [SerializeField] protected float _rotationSpeed = 10f;
    public Vector3 _offsetTarget;
    public float height = 4f;
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

        Vector3 velocity = Vector3.zero;
        if(_target.TryGetComponent(out Rigidbody rigidbody))
        {
            velocity = rigidbody.velocity;
        }
        Vector3 startPosition = _startTrans.position;
        Vector3 targetPosition = _target.position + _offsetTarget + (velocity);
        //Vector3 dirVector = _target.transform.forward

        // Path를 위한 waypoints 설정
        Vector3[] path = new Vector3[3];
        path[0] = startPosition;
        path[1] = new Vector3((startPosition.x + targetPosition.x) / 2, height, (startPosition.z + targetPosition.z) / 2);
        path[2] = targetPosition;

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLookAt(0.01f));
        sequence.Append(transform.DOMove(transform.position + (transform.forward * 4f),1f));
        sequence.AppendCallback(() => { Invoke(nameof(Destroy), 3f); });
    }

    protected override void Destroy()
    {
        Managers.Resource.Destroy(gameObject);
    }

    protected override void HandleImpact(Collider[] others)
    {
        base.HandleImpact(others);
        Invoke(nameof(Destroy), 3f);
        _projectileCollider.enabled = false;
        transform.SetParent(others[0].transform);
        sequence.Kill();
    }


}
