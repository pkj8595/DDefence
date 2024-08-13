using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;


public static class AffectFactory
{
    public static IAffect CreateAffect(Define.EAffectType affectType, float value)
    {
        switch (affectType)
        {
            case Define.EAffectType.Damage: // DamageAffect
                return new DamageAffect(value);
            case Define.EAffectType.Heal: // BuffAffect
                return null;
            case Define.EAffectType.Buff:
                return new MeleeDamageBuffAffect(value,1000);
            case Define.EAffectType.Debuff:
                return null;
            default:
                return null;
        }
    }
}
public interface IAffect
{
    void ApplyAffect(PawnStat target);
    void Remove(PawnStat target);
    bool IsExpired();
}

public abstract class TimedAffect : IAffect
{
    protected float duration;
    protected float startTime;

    public TimedAffect(float duration)
    {
        this.duration = duration;
        this.startTime = Time.time;
    }

    public abstract void ApplyAffect(PawnStat target);

    public virtual void Remove(PawnStat target)
    {
    }

    public bool IsExpired()
    {
        return Time.time >= startTime + duration;
    }

    
}

public class DamageAffect : IAffect
{
    public float DamageValue { get; private set; }

    public DamageAffect(float damageValue)
    {
        DamageValue = damageValue;
    }

    public void ApplyAffect(PawnStat target)
    {
        // 예시: target의 체력을 줄이는 로직
        var healthComponent = target.GetComponent<PawnStat>();
        if (healthComponent != null)
        {
            healthComponent.OnAttacked(DamageValue);
        }
    }

    public bool IsExpired()
    {
        return false;
    }

    public void Remove(PawnStat target)
    {
    }
}

public class MeleeDamageBuffAffect : TimedAffect
{
    public float BuffValue { get; private set; }
    private PawnStat _target;

    public MeleeDamageBuffAffect(float buffValue, float duration) : base(duration)
    {
        BuffValue = buffValue;
    }

    public override void ApplyAffect(PawnStat target)
    {
        var stat = target.CombatStat;
        stat.meleeDamage += BuffValue;
        target.CombatStat = stat;
        _target = target;
        target.StartCoroutine(RemoveCoroutine());
    }

    public override void Remove(PawnStat target)
    {
        base.Remove(target);
        var stat = target.CombatStat;
        stat.meleeDamage -= BuffValue;
        target.CombatStat = stat;
    }

    private IEnumerator RemoveCoroutine()
    {
        while (true)
        {
            yield return YieldCache.WaitForSeconds(1);
            if (IsExpired())
            {
                Remove(_target);
                break;
            }
        }
    }

    
}
