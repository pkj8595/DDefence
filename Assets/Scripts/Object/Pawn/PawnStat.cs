using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStat : Stat
{
    /// <summary>
    /// 전투 스탯
    /// </summary>
    [SerializeField]
    private CombatStat _combatStat;
    /// <summary>
    /// 캐릭터 스탯 
    /// </summary>
    [SerializeField]
    private BaseStat _currentBaseStat;

    private Data.StatData _baseStat;
    private readonly List<Data.StatData> _runeStatList = new List<Data.StatData>(Define.Rune_Count);
    private readonly List<Data.StatData> _traitStatList = new List<Data.StatData>(Define.Trait_Count);

    [field: SerializeField] public int KillCount { get; set; }
    [field: SerializeField] public int WaveCount { get; set; }
    public override float MaxHp { get => _combatStat.maxHp; }
    public override float MaxMana { get => _combatStat.maxMana; }
    public override float Protection { get => _combatStat.protection;}
    public int EXP { get { return KillCount + (WaveCount * 10); }}
    public float MoveSpeed { get { return _combatStat.movementSpeed; } }
    public CombatStat CombatStat { get => _combatStat; set => _combatStat = value; }

    
    public override void Init(int statDataNum, System.Action onDead, System.Action onDeadTarget)
    {
        base.Init(statDataNum, onDead, onDeadTarget);

        SetBaseStat(Managers.Data.StatDict[statDataNum]);
        CalculateCombatStat();
        Hp = _combatStat.maxHp;
        Mana = _combatStat.maxMana;
    }

    #region ChangeStat
    private void SetBaseStat(Data.StatData statData)
    {
        _baseStat = statData;
        CalculateCombatStat();
    }

    public void AddRuneStat(Data.StatData statData)
    {
        if (_runeStatList.Count <= Define.Rune_Count)
        {
            _runeStatList.Add(statData);
            CalculateCombatStat();
        }
    }

    public void RemoveRuneStat(Data.StatData statData)
    {
        if (_runeStatList.Remove(statData))
        {
            CalculateCombatStat();
        }
    }

    public void AddTraitStat(Data.StatData statData)
    {
        if (_traitStatList.Count <= Define.Trait_Count)
        {
            _traitStatList.Add(statData);
            CalculateCombatStat();
        }
    }

    public void RemoveTraitStat(Data.StatData statData)
    {
        if (_traitStatList.Remove(statData))
        {
            CalculateCombatStat();
        }
    }

    private void CalculateCombatStat()
    {
        _currentBaseStat.Reset();
        _currentBaseStat += _baseStat;
        for (int i = 0; i < _runeStatList.Count; i++)
        {
            if (_runeStatList[i] != null)
                _currentBaseStat += _runeStatList[i];
        }
        for (int i = 0; i < _traitStatList.Count; i++)
        {
            if (_traitStatList[i] != null)
                _currentBaseStat += _traitStatList[i];
        }
        _combatStat = CombatStat.ConvertStat(_currentBaseStat);
    }
    #endregion


    public override float GetAttackValue(Define.EDamageType damageType)
    {
        // ((Attack * 밸런스) * skill value) )  * (randomValue > 80) ? 1f : 1.5f
        float damageTypeValue = 0f;
        switch (damageType)
        {
            case Define.EDamageType.Melee:
                damageTypeValue = CombatStat.meleeDamage;
                break;
            case Define.EDamageType.Ranged:
                damageTypeValue = CombatStat.rangedDamage;
                break;
            case Define.EDamageType.Magic:
                damageTypeValue = CombatStat.magicDamage;
                break;
            default:
                Debug.LogError($"EDamageType.{damageType}이 케이스에 없습니다.");
                break;
        }
        float baseBalance = 50f;
        float balanceValue = Random.Range(baseBalance + (CombatStat.balance * 0.5f), 100f) * 0.01f;
        float ret = damageTypeValue * 
                    balanceValue * 
                    (Random.Range(0, 100) < CombatStat.criticalHitChance ? 1f : 1.5f);
        
        return ret;
    }

    public override void OnAttacked(float damageAmount, Stat attacker)
    {
        // 회피스탯 적용
        if (Random.Range(0, 1000) < _combatStat.dodgepChance)
        {
            Debug.Log("회피");
            //todo effectManager text
            return;
        }

        //todo effectManager damageNum
        float damage = Mathf.Max(0, CalculateDamage(damageAmount, _combatStat.protection));
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    public override void OnDeadTarget()
    {
        base.OnDeadTarget();
        KillCount++;
    }

    public void IncreadMana()
    {
        Mana += _combatStat.manaRegeneration;
    }

}
