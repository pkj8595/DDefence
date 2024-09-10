using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력바가 있는 모든 오브젝트에 사용
/// </summary>
public interface IStat
{
    public float Hp { get; set; }
    public float Mana { get; set; }
    public float MaxHp { get;}
    public float MaxMana { get;}

}

public abstract class Stat : MonoBehaviour, IStat
{
    [SerializeField] private float _hp;
    [SerializeField] private float _mana;

    public float Hp
    {
        get => _hp; 
        set
        {
            _hp = value < MaxHp ? value : MaxHp;
        }
    }

    public float Mana 
    {
        get => _mana;
        set
        {
            _mana = value < MaxMana ? value : MaxMana;
        }
    }
    public abstract float MaxHp { get;}
    public abstract float MaxMana { get;}
    public abstract float Protection { get;}
    public bool IsDead { get; protected set; }

    protected System.Action _OnDeadEvent;
    protected System.Action _OnDeadTargetEvent;
    protected System.Action _OnAffectEvent;
    protected List<AffectBase> _affectList = new List<AffectBase>();


    public virtual void Init(int statDataNum, System.Action onDead, System.Action onDeadTarget)
    {
        _OnDeadEvent = onDead;
        _OnDeadTargetEvent = onDeadTarget;
        IsDead = false;
        StartCoroutine(UpdateAffect());
    }

    /// <summary>
    /// 피격시 실행되는 로직
    /// </summary>
    /// <param name="msg"></param>
    public virtual void ApplyDamageMessage(ref DamageMessage msg)
    {
        //affect 실행
        ApplyAffect(msg.skillAffectList, msg.attacker);
    }

    protected virtual void ApplyAffect(AffectBase[] affects, Stat attacker)
    {
        for (int i = 0; i < affects.Length; i++)
        {
            affects[i]?.ApplyAffect(attacker, this);
        }
    }

    public void SetAffectEvent(System.Action affectAction)
    {
        _OnAffectEvent -= affectAction;
        _OnAffectEvent += affectAction;
    }
    public void RemoveAffectEvent(System.Action affectAction)
    {
        _OnAffectEvent -= affectAction;
    }

    protected IEnumerator UpdateAffect()
    {
        while (!IsDead)
        {
            for (int i = _affectList.Count - 1; i >= 0; i--)
            {
                if (_affectList[i].IsExpired())
                {
                    _affectList[i].Remove();
                    _affectList.RemoveAt(i);
                }
            }
            _OnAffectEvent?.Invoke();
            yield return YieldCache.WaitForSeconds(1f);
        }
    }

    //Attack Value
    public abstract float GetAttackValue(Define.EDamageType damageType);
    

    //공격 당했을때
    public virtual void OnAttacked(float damageAmount, Stat attacker) 
    {
        float damage = Mathf.Max(0, CalculateDamage(damageAmount, Protection));
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    protected float CalculateDamage(float damage, float protection)
    {
        if (0 <= protection)
        {
            return (float)(damage / (1f + (protection * 0.01f)));
        }
        else
        {
            return damage;
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        if (attacker != null)
        {
            attacker.OnDeadTarget();
        }

        IsDead = true;
        _OnDeadEvent?.Invoke();
    }

    //상대가 죽었을때 호출
    public virtual void OnDeadTarget()
    {
        _OnDeadTargetEvent.Invoke();

    }

    public void OnLive()
    {
        Hp += MaxHp * 0.5f;
        IsDead = false;
    }

    public virtual void DontSpendCost()
    {
        Hp -= MaxHp * 0.2f;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(null);
        }
    }

    public virtual void SpendCost()
    {
        Hp += MaxHp * 0.2f;
    }
    public virtual float GetSkillCooldown()
    {
        return 0;
    }

    public virtual float GetBaseSkillCooldown()
    {
        return 0;
    }
}