using UnityEngine;
using System;

[RequireComponent(typeof(Poolable))]
public abstract class ProjectileBase : MonoBehaviour
{
    [SerializeField] protected float _speed = 10f;
    [SerializeField] protected float _rotationSpeed = 10f;
    protected DamageMessage _msg;
    /// <summary>
    /// 타격 사운드
    /// </summary>
    public const string effectSoundName = "";

    // 발사체가 타겟에 충돌했을 때 호출
    protected virtual void OnTriggerEnter(Collider other)
    {
        //_msg의 레이어가 아닐경우 return
        //if (!Utils.CompareFlag(other.gameObject.layer, _msg.targetLayer))
        //    return;

        if (!string.IsNullOrEmpty(effectSoundName))
            Managers.Sound.Play(effectSoundName, Define.Sound.Effect);

        HandleImpact(other);
    }

    // 충돌 처리 로직 (파생 클래스에서 구현)
    protected abstract void HandleImpact(Collider other);

    protected abstract void Destroy();
}
