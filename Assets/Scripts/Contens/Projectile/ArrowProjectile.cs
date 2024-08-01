using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArrowProjectile : ProjectileBase
{
    [SerializeField] private Transform _target;
    public ParticleSystem _effHit;
    public Vector3 _offsetTarget;
    public float height = 8f;
    public float duration = 1f;
    public bool isStart = false;

    public void Init(Transform target, DamageMessage msg)
    {
        _target = target;
        _msg = msg;
        Shot();
    }

    public void Shot()
    {
        if (_target == null) return;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = _target.position + _offsetTarget;
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

    private void OnEnable()
    {
        Shot();
    }

    protected override void Destroy()
    {
        Managers.Resource.Destroy(gameObject);
    }

    protected override void HandleImpact(Collider other)
    {
        Debug.Log("HandleImpact");
        if (_effHit != null)
        {
            _effHit.gameObject.SetActive(true);
            _effHit.Play();
        }

        if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ApplyTakeDamege(_msg);
        }

        Invoke(nameof(Destroy), 3f);
    }


}
