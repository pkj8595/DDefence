using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public Stat attacker;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public AffectBase[] skillAffectList;
    public Skill skill;

    public DamageMessage(Stat attacker, Vector3 hitPoint, Vector3 hitNormal, Skill skill)
    {
        this.attacker = attacker;
        this.hitPoint = hitPoint;
        this.hitNormal = hitNormal;
        this.skill = skill;
        this.skillAffectList = skill.AffectList.ToArray();
    }
}
/*
데미지 계산식
damage = ((Attack * 밸런스) * 크리 * skill value) ) 

받는쪽 = 회피 판별 => damage - defence 




 */