using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStat : MonoBehaviour, Stat
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
    [field: SerializeField] public float Hp { get; set; }
    [field: SerializeField] public float Mana { get; set; }
    public float MaxHp { get => _combatStat.maxHp; }
    public float MaxMana { get => _combatStat.maxMana; }
    public int EXP { get { return KillCount + (WaveCount * 10); }}
    public bool IsDead { get; set; }
    public float MoveSpeed { get { return _combatStat.movementSpeed; } }
    public CombatStat CombatStat { get => _combatStat; set => _combatStat = value; }

    private System.Action _OnDeadEvent;
    private System.Action _OnDeadTargetEvent;

    private System.Action _OnAffectEvent;
    private List<AffectBase> _affectList = new List<AffectBase>();

    public void Init(int statDataNum, System.Action onDead, System.Action onDeadTarget)
    {
        SetBaseStat(Managers.Data.StatDict[statDataNum]);
        CalculateCombatStat();
        Hp = _combatStat.maxHp;
        Mana = _combatStat.maxMana;
        _OnDeadEvent = onDead;
        _OnDeadTargetEvent = onDeadTarget;

        StartCoroutine(UpdateAffect());
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

    public void SetAffectEvent(System.Action affectAction)
    {
        _OnAffectEvent -= affectAction;
        _OnAffectEvent += affectAction;
    }
    public void RemoveAffectEvent(System.Action affectAction)
    {
        _OnAffectEvent -= affectAction;
    }

    IEnumerator UpdateAffect()
    {
        while (!IsDead)
        {
            for (int i = _affectList.Count - 1; i >= 0; i--)
            {
                if (_affectList[i].IsExpired())
                {
                    _affectList[i].Remove();
                    _affectList.RemoveAt(i);
                }
            }
            _OnAffectEvent?.Invoke();
            yield return YieldCache.WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// 피격시 실행되는 로직
    /// </summary>
    /// <param name="msg"></param>
    public virtual void ApplyDamageMessage(ref DamageMessage msg)
    {
        //affect 실행
        ApplyAffect(msg.skillAffectList, msg.attacker);
    }

    private void ApplyAffect(AffectBase[] affects, PawnStat attacker)
    {
        for (int i = 0; i < affects.Length; i++)
        {
            affects[i]?.ApplyAffect(attacker, this);
        }
    }

    
    public float GetAttackValue(Define.EDamageType damageType)
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

    public virtual void OnAttacked(float damageAmount, PawnStat attacker)
    {
        // 회피스탯 적용
        if (Random.Range(0, 1000) < 200 + (_combatStat.dodgepChance - attacker.CombatStat.dodgepEnetration))
        {
            Debug.Log("회피");
            //todo effectManager text
            return;
        }

        //todo effectManager damageNum
        float damage = Mathf.Max(0, damageAmount - _combatStat.protection);
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    private void OnDead(PawnStat attacker)
    {
        if (attacker != null)
        {
            attacker.KillCount++;
            attacker.OnDeadTarget();
        }
        IsDead = true;
        _OnDeadEvent?.Invoke();
    }

    public void OnDeadTarget()
    {
        _OnDeadTargetEvent.Invoke();
    }

    public void IncreadMana()
    {
        Mana += _combatStat.manaRegeneration;
    }

}
