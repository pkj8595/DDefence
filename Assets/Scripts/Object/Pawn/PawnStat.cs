using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStat : MonoBehaviour
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
    private List<Data.StatData> _runeStatList = new List<Data.StatData>(Define.runeCount);
    private List<Data.StatData> _traitStatList = new List<Data.StatData>(Define.traitCount);

    [field: SerializeField] public int KillCount { get; set; }
    [field: SerializeField] public int WaveCount { get; set; }
    [field: SerializeField] public float Hp { get; set; }
    [field: SerializeField] public float Mana { get; set; }
    public int EXP { get { return KillCount + (WaveCount * 10); }}
    public bool IsDead { get; set; }

    public float MoveSpeed { get { return _combatStat.movementSpeed; } }

    public CombatStat CombatStat { get => _combatStat; set => _combatStat = value; }

    public void Init(int statDataNum)
    {
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
        if (_runeStatList.Count <= Define.runeCount)
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
        if (_traitStatList.Count <= Define.traitCount)
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

    /// <summary>
    /// 피격시 실행되는 로직
    /// </summary>
    /// <param name="msg"></param>
    public virtual void OnAttacked(DamageMessage msg)
    {
        // 회피스탯 적용
        if (Random.Range(0, 1000) < 200)
        {
            Debug.Log("회피");
            return;
        }

        //todo 
        int damage = (int)Mathf.Max(0, msg.damageAmount - _combatStat.protection);
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(msg.attacker);
        }
    }

    public virtual void OnAttacked(float damage)
    {
        
    }

    private void OnDead(PawnStat attacker)
    {
        if (attacker != null)
        {
            attacker.KillCount++;
        }
        IsDead = true;
        //Managers.Game.Despawn(gameObject);
    }

    public void ApplyAffect(IAffect affect)
    {
        affect.ApplyAffect(this);
    }
    
  
}
