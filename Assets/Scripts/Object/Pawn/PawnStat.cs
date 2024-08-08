using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnStat : MonoBehaviour
{
    public Data.StatData _curruntStat;
    public Data.StatData _baseStat;
    public Data.StatData[] _equipmentStatList = new Data.StatData[3]; // todo

    private int _statDataBaseNum;
    
    [field: SerializeField] public int KillCount { get; set; }
    [field: SerializeField] public int WaveCount { get; set; }

    //todo stat클래스 정리
    [field: SerializeField] public int Hp { get; set; }
    [field: SerializeField] public int Mana { get; set; }
    [field: SerializeField] public int Level { get; set; }

    private int _exp;
    public bool IsDead { get; set; }

    public int MaxHp { get => _curruntStat.maxHp; }
    public int MaxMana { get => _curruntStat.maxMana; }
    public int Attack { get => _curruntStat.attack; }
    public int Defense { get => _curruntStat.defense; }
    public float MoveSpeed { get => _curruntStat.moveSpeed; }
    public float AttackSpeed { get => _curruntStat.attackSpeed; }
    public float AttackRange { get => _curruntStat.attackRange; }
    public float Bellruns { get => _curruntStat.bellruns; }
    public int TotalExp { get => _curruntStat.totalExp; }
    public int DropExp { get => _curruntStat.dropExp; }

    //exp 를 
    public int Exp
    {
        get => _exp;
        private set
        {
            _exp = value;
            int level = Level;
            while (true)
            {
                if (Managers.Data.StatDict.TryGetValue(CalculateStatDataNum(), out Data.StatData stat) == false)
                    break;
                if (_exp < stat.totalExp)
                    break;
                level++;
            }
            if (level != Level)
            {
                Debug.Log($"Level Up {Level}");
                Level = level;
                SetStat(CalculateStatDataNum());
            }
        }
    }

    public void Init(int statDataNum)
    {
        _statDataBaseNum = Utils.CalculateTableBaseNumber(statDataNum);
        Level = 0;
        _exp = 0;
        SetStat(CalculateStatDataNum());
    }

    private void SetStat(Data.StatData statData)
    {
        _curruntStat = statData;
        Hp = statData.maxHp;
        Mana = statData.maxMana;
        IsDead = false;
    }

    private void SetStat(int statDataNum)
    {
        SetStat(Managers.Data.StatDict[statDataNum]);
    }

    private int CalculateStatDataNum()
    {
        return _statDataBaseNum + Level;
    }

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
        int damage = (int)Mathf.Max(0, msg.damageAmount - Defense);
        Hp -= damage;
        if (Hp < 0)
        {
            Hp = 0;
            OnDead(msg.attacker);
        }
    }

    //벨런스 어택 데미지 산출
    public virtual float GetAttackValue()
    {
        const float bellMaxValue = 1.0f;
        const float bellMinValue = 0.01f;

        float bell = Mathf.Clamp(Bellruns, bellMinValue, bellMaxValue);
        return Random.Range(Attack * bell, Attack);
    }

    private void OnDead(PawnStat attacker)
    {
        if (attacker != null)
        {
            attacker.Exp += DropExp;
        }
        IsDead = true;
        Managers.Game.Despawn(gameObject);
    }

    

}
