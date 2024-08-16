using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;


public static class AffectFactory
{
    public static AffectBase CreateAffect(Data.SkillAffectData affectData)
    {
        return affectData.affectType switch
        {
            Define.EAffectType.Damage => new DamageAffect(affectData),
            Define.EAffectType.Heal => null,
            Define.EAffectType.Buff => new MeleeDamageBuffAffect(affectData),
            Define.EAffectType.Debuff => null,
            _ => null,
        };
    }
}

public abstract class AffectBase
{
    protected Data.SkillAffectData _data;
    public AffectBase(Data.SkillAffectData data)
    {
        _data = data;
    }
    public abstract void ApplyAffect(PawnStat attecker, PawnStat taker);
    public abstract void Remove();
    public abstract bool IsExpired();
}

public abstract class TimedAffect : AffectBase
{
    protected float startTime;

    protected TimedAffect(Data.SkillAffectData data) : base(data)
    {
    }

    public override void ApplyAffect(PawnStat attecker, PawnStat taker) { }

    public override void Remove()
    {
    }

    public override bool IsExpired()
    {
        return Time.time >= startTime + _data.value;
    }

    
}

public class DamageAffect : AffectBase
{
    public float DamageValue { get; private set; }

    public DamageAffect(Data.SkillAffectData data) :base (data)
    {
        DamageValue = data.value;
    }

    public override void ApplyAffect(PawnStat attacker, PawnStat taker)
    {
        // 예시: target의 체력을 줄이는 로직
        var healthComponent = taker.GetComponent<PawnStat>();
        if (healthComponent != null)
        {
            healthComponent.OnAttacked((DamageValue * 0.01f) * attacker.GetAttackValue(_data.damageType), attacker);
        }
    }

    public override bool IsExpired()
    {
        return true;
    }

    public override void Remove()
    {
    }
}

public class MeleeDamageBuffAffect : TimedAffect
{
    public float BuffValue { get; private set; }
    private PawnStat _target;

    public MeleeDamageBuffAffect(Data.SkillAffectData data) : base(data)
    {
        BuffValue = data.value;
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
        if (IsExpired())
        {
            Remove();
        }
    }

    
}
