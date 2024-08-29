using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStat : Stat
{
    private Data.BuildingData _data;
    public float DamageValue => _data.damageValue;
    public float ManaRegeneration => _data.manaRegeneration;
    public override float MaxHp => _data.maxHp;
    public override float MaxMana => _data.maxMana;
    public override float Protection => _data.protection;
    

    public override void Init(int statDataNum, System.Action onDead, System.Action onDeadTarget)
    {
        base.Init(statDataNum, onDead, onDeadTarget);
        _data = Managers.Data.BuildingDict[statDataNum];
        Mana = MaxMana;
        Hp = MaxHp;
    }

    public override float GetAttackValue(Define.EDamageType damageType)
    {
        return DamageValue;
    }

    public void IncreadMana()
    {
        Mana += ManaRegeneration;
    }
}
