using UnityEngine;
using System;

[RequireComponent(typeof(Poolable))]
public abstract class ProjectileBase : MonoBehaviour
{
    public ParticleSystem _effHit;
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected Collider _projectileCollider;
    [SerializeField] protected bool IsPenetration = false;

    protected DamageMessage _msg;
    protected Transform _target;
    protected Transform _startTrans;
    protected float _splashRange;
    protected int _count = 0;
    /// <summary>
    /// 타격 사운드
    /// </summary>
    public const string effectSoundName = "";

    public virtual void Init(Transform startPosition, Transform target, float splashRange, in DamageMessage msg)
    {
        this.transform.SetPositionAndRotation(startPosition.position, startPosition.rotation);
        _startTrans = startPosition;
        _target = target;
        _splashRange = splashRange;
        _msg = msg;
        _count = 0;
        if (_projectileCollider == null)
            _projectileCollider = GetComponent<Collider>();
        _projectileCollider.enabled = true;
        _projectileCollider.isTrigger = true;
    }

    // 발사체가 타겟에 충돌했을 때 호출
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || _startTrans == other.transform)
            return;

        if (!IsPenetration && 0 < _count)
            return;

        _count++;

        if (!string.IsNullOrEmpty(effectSoundName))
            Managers.Sound.Play(effectSoundName, Define.Sound.Effect);

        Collider[] colliders;
        if (_splashRange == 0)
        {
            colliders = new Collider[1];
            colliders[0] = other;
        }
        else 
        {
            colliders = Physics.OverlapSphere(transform.position, _splashRange);
        }
        HandleImpact(colliders);
    }

    // 충돌 처리 로직 (파생 클래스에서 구현)
    protected virtual void HandleImpact(Collider[] others)
    {
        for (int i = 0; i < others.Length; i++)
        {
            if (others[i].gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyTakeDamage(_msg);
                PlayEffect();
            }
        }
    }

    protected abstract void Destroy();
    protected virtual void PlayEffect()
    {
        if (_effHit != null)
        {
            _effHit.gameObject.SetActive(true);
            _effHit.Play();
        }
    }
}
