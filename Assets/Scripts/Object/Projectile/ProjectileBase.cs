using UnityEngine;
using System;

[RequireComponent(typeof(Poolable))]
public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _rotationSpeed = 10f;
    protected DamageMessage _msg;
    protected Transform _target;
    protected Transform _startTrans;
    protected float _splashRange;
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
    }

    // 발사체가 타겟에 충돌했을 때 호출
    protected virtual void OnTriggerEnter(Collider other)
    {
        //_msg의 레이어가 아닐경우 return
        //if (!Utils.CompareFlag(other.gameObject.layer, _msg.targetLayer))
        //    return;
        if (_target != other.transform)
            return;

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
            int layer = (int)Define.Layer.Pawn | (int)Define.Layer.Building;
            colliders = Physics.OverlapSphere(transform.position, _splashRange, layer);
        }
        HandleImpact(colliders);
    }

    // 충돌 처리 로직 (파생 클래스에서 구현)
    protected abstract void HandleImpact(Collider[] other);

    protected abstract void Destroy();
}
