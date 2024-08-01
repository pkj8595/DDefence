using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageMessage
{
    public GameObject attacker;
    public Stat target;
    public float damageAmount;
    public Vector3 hitPoint;
    public Vector3 hitNormal;
    public int targetLayer;
}
/*
데미지 계산식
damage = ((Attack * 밸런스) * 크리 * skill value) ) 

받는쪽 = 회피 판별 => damage - defence 

 */