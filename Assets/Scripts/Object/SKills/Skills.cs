using System.Collections.Generic;
using UnityEngine;

// 기본 스킬 클래스
public class Skill
{
    private Data.SkillData data;
    public string Icon { get => data.icon; }
    public string Name { get => data.name; }
    public string Desc { get => data.desc; }
    public Define.ETargetType TargetType { get => data.targetType; }
    public float ManaAmount { get => data.manaAmount; }
    public float MinRange { get => data.minRange; }
    public float MaxRange { get => data.maxRange; }
    public float SplashRange { get => data.splashRange; }
    public float CoolTime { get => data.coolTime; }
    public string MotionAni { get => data.motionAni; }
    public float MotionDuration { get => data.motionDuration; }
    public string EffectStr { get => data.effectStr; }


    public List<IAffect> effects = new List<IAffect>(5);
    public float LastRunTime { get; set; }
    private PawnStat _attacker;

    public void Init(int skillTableNum)
    {
        data = Managers.Data.SkillDict[skillTableNum];
        LastRunTime = 0f;
    }

    public void ExcuteSkill(PawnStat attacker)
    {
        LastRunTime = Time.time;
        _attacker = attacker;
    }

    public void ApplyEffects(IDamageable target)
    {
        foreach (IAffect effect in effects)
        {
            effect.Apply(target);
        }
    }

    public bool IsCooltime()
    {
        bool isCooltime = Time.time < LastRunTime + CoolTime;

        return isCooltime;
    }

    public ProjectileBase MakeProjectile()
    {
        if (string.IsNullOrEmpty(data.projectile))
            return null;

        return Managers.Resource.Instantiate(Define.Path.Prefab_Bullet + data.projectile).GetComponent<ProjectileBase>();
    }
}
