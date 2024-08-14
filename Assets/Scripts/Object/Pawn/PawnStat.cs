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
    private readonly List<Data.StatData> _runeStatList = new List<Data.StatData>(Define.Rune_Count);
    private readonly List<Data.StatData> _traitStatList = new List<Data.StatData>(Define.Trait_Count);

    [field: SerializeField] public int KillCount { get; set; }
    [field: SerializeField] public int WaveCount { get; set; }
    [field: SerializeField] public float Hp { get; set; }
    [field: SerializeField] public float Mana { get; set; }
    public int EXP { get { return KillCount + (WaveCount * 10); }}
    public bool IsDead { get; set; }
    public float MoveSpeed { get { return _combatStat.movementSpeed; } }
    public CombatStat CombatStat { get => _combatStat; set => _combatStat = value; }

    private System.Action _OnDeadEvent;

    private System.Action _OnAffectEvent;
    private List<IAffect> _affectList = new List<IAffect>();

    public void Init(int statDataNum, System.Action onDead)
    {
        SetBaseStat(Managers.Data.StatDict[statDataNum]);
        CalculateCombatStat();
        Hp = _combatStat.maxHp;
        Mana = _combatStat.maxMana;
        _OnDeadEvent = onDead;

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
            for (int i = _affectList.Count; i >= 0; i--)
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

    private void ApplyAffect(IAffect[] affects, PawnStat attacker)
    {
        for (int i = 0; i < affects.Length; i++)
        {
            affects[i]?.ApplyAffect(attacker, this);
        }
    }

    public virtual void OnAttacked(float damageAmount, PawnStat attacker)
    {
        // 회피스탯 적용
        if (Random.Range(0, 1000) > 200 + (_combatStat.dodgepChance - attacker.CombatStat.dodgepEnetration))
        {
            Debug.Log("회피");
            //todo effectManager text
            return;
        }

        //todo effectManager damageNum
        int damage = (int)Mathf.Max(0, damageAmount - _combatStat.protection);
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
        }
        IsDead = true;
        _OnDeadEvent?.Invoke();
    }

}
