using System.Collections.Generic;
using UnityEngine;

// 기본 스킬 클래스
public class Skill
{
    public string skillImg;
    public string skillName;
    public string description;
    public float cooldown;
    public float lastRunTime; // 

    public List<SkillEffect> effects = new List<SkillEffect>();

    public void ApplyEffects(GameObject target)
    {
        foreach (var effect in effects)
        {
            effect.ApplyEffect(target);
        }
    }

    public bool IsExcute()
    {
        bool isExcute = Time.time > lastRunTime + cooldown;
        if (isExcute)
            lastRunTime = Time.time;

        return isExcute;
    }
}

public abstract class SkillEffect
{
    public float duration;
    public float amount;
    public abstract void ApplyEffect(GameObject target);
}

public class BuffEffect : SkillEffect
{
    public float buffAmount;

    public override void ApplyEffect(GameObject target)
    {
        // 버프 효과 적용 로직
        Debug.Log($"Applying Buff to {target.name} with amount {buffAmount}");
    }
}

public class DebuffEffect : SkillEffect
{
    public float debuffAmount;

    public override void ApplyEffect(GameObject target)
    {
        // 디버프 효과 적용 로직
        Debug.Log($"Applying Debuff to {target.name} with amount {debuffAmount}");
    }
}

public class DamageEffect : SkillEffect
{
    public int damageAmount;

    public override void ApplyEffect(GameObject target)
    {
        // 타격 효과 적용 로직
        Debug.Log($"Dealing {damageAmount} damage to {target.name}");
    }
}

public class HealEffect : SkillEffect
{
    public int healAmount;

    public override void ApplyEffect(GameObject target)
    {
        // 힐 효과 적용 로직
        Debug.Log($"Healing {healAmount} health to {target.name}");
    }
}

public class SlowEffect : SkillEffect
{
    public float slowPercentage;

    public override void ApplyEffect(GameObject target)
    {
        // 슬로우 효과 적용 로직
        Debug.Log($"Slowing {target.name} by {slowPercentage}% for {duration} seconds");
    }
}

public class PoisonEffect : SkillEffect
{
    public int poisonDamage;
    public float poisonInterval;

    public override void ApplyEffect(GameObject target)
    {
        // 중독 효과 적용 로직
        Debug.Log($"Applying poison to {target.name} with {poisonDamage} damage every {poisonInterval} seconds for {duration} seconds");
    }
}

public class StunEffect : SkillEffect
{
    public override void ApplyEffect(GameObject target)
    {
        // 스턴 효과 적용 로직
        Debug.Log($"Stunning {target.name} for {duration} seconds");
    }
}

/*
// 스킬 시스템 사용 예제
public class SkillSystem : MonoBehaviour
{
    public List<Skill> skills = new List<Skill>();

    void Start()
    {
        // 예제 스킬 추가
        skills.Add(new BuffSkill() { skillName = "Power Up", description = "Increases attack power", cooldown = 5f, duration = 10f, buffAmount = 10f });
        skills.Add(new DebuffSkill() { skillName = "Weakness", description = "Decreases enemy defense", cooldown = 5f, duration = 10f, debuffAmount = 10f });
        skills.Add(new DamageSkill() { skillName = "Fireball", description = "Deals fire damage", cooldown = 3f, damageAmount = 30 });
        skills.Add(new HealSkill() { skillName = "Heal", description = "Restores health", cooldown = 5f, healAmount = 20 });
        skills.Add(new SlowSkill() { skillName = "Ice Blast", description = "Slows enemy", cooldown = 4f, duration = 5f, slowPercentage = 50f });
        skills.Add(new PoisonSkill() { skillName = "Poison Cloud", description = "Poisons enemy over time", cooldown = 6f, duration = 8f, poisonDamage = 5, poisonInterval = 1f });
        skills.Add(new StunSkill() { skillName = "Stun", description = "Stuns enemy", cooldown = 7f, duration = 2f });

        // 스킬 사용 예제
        GameObject target = new GameObject("TargetDummy");
        foreach (var skill in skills)
        {
            skill.ApplyEffect(target);
        }
    }
}*/