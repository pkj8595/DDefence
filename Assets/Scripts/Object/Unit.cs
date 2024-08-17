using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력바가 있는 모든 오브젝트에 사용
/// </summary>
public interface Stat
{
    public float Hp { get; set; }
    public float Mana { get; set; }
    public float MaxHp { get;}
    public float MaxMana { get;}
}
