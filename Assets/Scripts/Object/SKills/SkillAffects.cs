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
    void ApplyAffect(PawnStat attecker, PawnStat taker);
    void Remove();
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

    public abstract void ApplyAffect(PawnStat attecker, PawnStat taker);

    public virtual void Remove()
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

    public void ApplyAffect(PawnStat attacker, PawnStat taker)
    {
        // 예시: target의 체력을 줄이는 로직
        var healthComponent = taker.GetComponent<PawnStat>();
        if (healthComponent != null)
        {
            healthComponent.OnAttacked(DamageValue * attacker.CombatStat.meleeDamage, attacker);
        }
    }

    public bool IsExpired()
    {
        return true;
    }

    public void Remove()
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

    public override void ApplyAffect(PawnStat attecker, PawnStat taker)
    {
        _target = taker;
        var stat = taker.CombatStat;
        stat.meleeDamage += BuffValue;
        taker.CombatStat = stat;
        taker.SetAffectEvent(UpdateAction);
    }

    public override void Remove()
    {
        base.Remove();
        var stat = _target.CombatStat;
        stat.meleeDamage -= BuffValue;
        _target.CombatStat = stat;
        _target.RemoveAffectEvent(UpdateAction);
    }

    private void UpdateAction()
    {
        while (true)
        {
            if (IsExpired())
            {
                Remove();
                break;
            }
        }
    }

    
}
