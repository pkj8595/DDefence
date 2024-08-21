using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowProjectile : ProjectileBase
{
    public ParticleSystem _effHit;
    public Vector3 _offsetTarget;
    public float height = 4f;
    public float duration = 1f;
    public bool isAffect = false;

    public override void Init(Transform startTrans, Transform target, float splashRange, in DamageMessage msg)
    {
        base.Init(startTrans, target, splashRange, msg);
        isAffect = false;
        Shot();
    }

    public void Shot()
    {
        if (_target == null) return;

        Vector3 targetSpeed = Vector3.zero;
        if(_target.TryGetComponent(out Rigidbody rigidbody))
        {
            targetSpeed = rigidbody.velocity;
        }
        Vector3 startPosition = _startTrans.position;
        Vector3 targetPosition = _target.position + _offsetTarget + (targetSpeed);
        //Vector3 dirVector = _target.transform.forward

        // Path를 위한 waypoints 설정
        Vector3[] path = new Vector3[3];
        path[0] = startPosition;
        path[1] = new Vector3((startPosition.x + targetPosition.x) / 2, height, (startPosition.z + targetPosition.z) / 2);
        path[2] = targetPosition;

        // Path에 따라 이동 (LookAt을 사용하여 타겟을 바라봄)
        transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLookAt(0.01f);

    }



    protected override void Destroy()
    {
        Managers.Resource.Destroy(gameObject);
    }

    protected override void HandleImpact(Collider[] others)
    {
        if (!isAffect)
        {
            isAffect = true;
            if (others[0] != null)
            {
                if (others[0].gameObject.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyTakeDamege(_msg);
                    transform.SetParent(others[0].transform);
                    if (_effHit != null)
                    {
                        _effHit.gameObject.SetActive(true);
                        _effHit.Play();
                    }
                }
            }

        }
        Invoke(nameof(Destroy), 3f);
    }


}
